using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pfm.Models;
using pfm.Commands;
using System;
using Microsoft.AspNetCore.Http;
using pfm.Services;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using pfm.Models.Exceptions;

namespace pfm.Controllers{
    [ApiController]
    [Route("/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly IPfmService _pfmService;
        public TransactionsController(ILogger<TransactionsController> logger, IPfmService pfmService)
        {
            _logger = logger;
            _pfmService = pfmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery(Name ="transaction-kind")] string transactionKind, [FromQuery(Name ="start-date")] DateTime? startDate, [FromQuery(Name ="end-date")] DateTime? endDate, [FromQuery] int? page, [FromQuery(Name ="page-size")] int? pageSize, [FromQuery(Name ="sort-by")] string sortBy, [FromQuery(Name ="sort-order")] SortOrder sortOrder)
        {
            page ??= 1;
            pageSize ??= 10;
            var pagedSortedList = await _pfmService.GetTransactions(transactionKind, startDate, endDate, page.Value, pageSize.Value, sortBy, sortOrder);
            return Ok(pagedSortedList);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions([FromForm] IFormFile file)
        {
            var pom = await _pfmService.ImportTransactions(file);

            return Ok(pom);
        }

        [HttpPost("{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromRoute] string id, [FromBody] SplitTransactionCommand command)
        {
            try 
            {
                var res = await _pfmService.SplitTransaction(id, command);
                return Ok();
            }
            catch(ErrorException exception)
            {
                return BadRequest(exception.ValidationProblem.errors);
            }
        }

        [HttpPost("{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] TransactionCategorizeCommand command)
        {
            try
            {
                var res = await _pfmService.CategorizeTransaction(id, command);
                return Ok();
            }
            catch (ErrorException exception)
            {
                return BadRequest(exception.ValidationProblem.errors);
            }
        }

        [HttpPost("auto-categorize")]
        public IActionResult CategorizeTransactions()
        {
            return Ok();
        }
    }
}