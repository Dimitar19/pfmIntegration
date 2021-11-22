using Microsoft.AspNetCore.Http;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Models;
using pfm.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pfm.Services
{
    public interface IPfmService 
    {
        Task<TransactionPagedList<Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.asc);
        Task<CategoryList> GetCategories(string parentId);
        Task<List<Transaction>> ImportTransactions(IFormFile file);
        Task<List<Category>> ImportCategories(IFormFile file);
        Task<Transaction> CategorizeTransaction(string id, TransactionCategorizeCommand command);
        Task<SpendingsByCategory> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, Directions? direction);
        Task<ErrorException> SplitTransaction(string id, SplitTransactionCommand command);
    }
}