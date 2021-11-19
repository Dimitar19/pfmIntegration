using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Database.Repositories;
using pfm.Models;

namespace pfm.Services
{
    public class PfmService : IPfmService
    {
        private readonly IPfmRepository _pfmRepository;
        private readonly IMapper _mapper;
        public PfmService(IPfmRepository pfmRepository, IMapper mapper)
        {
            _pfmRepository = pfmRepository;
            _mapper = mapper;
        }
        public async Task<TransactionPagedList<Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.asc)
        {
            List<TransactionKind> transactionKinds = new List<TransactionKind>();
            if (!string.IsNullOrEmpty(transactionKind))
                foreach (var kind in transactionKind.Split(','))
                {
                    transactionKinds.Add((TransactionKind)Enum.Parse(typeof(TransactionKind), kind, true));
                }
            var pagedSortedList = await _pfmRepository.GetTransactions(transactionKinds, startDate, endDate, page, pageSize, sortBy, sortOrder);




            // //var query = _dbContext.Transactions.AsQueryable();
            // var pom = await _pfmRepository.GetTransactions();
            // var transactions = pom.AsQueryable();
            // var query = transactions.AsQueryable();


            // if(transactionKinds.Count > 0)
            //     transactions = transactions.Where(t=>transactionKinds.Contains(t.Kind));//query = query.Where(s => transactionKinds.Contains(s.Kind));
            // if(startDate != null)
            //     transactions = transactions.Where(t=>t.Date >= startDate);//query = query.Where(s=>s.Date >= startDate);
            // if(endDate != null)
            //     transactions = transactions.Where(t=>t.Date <= endDate);//query = query.Where(s=>s.Date <= endDate);

            // var total = transactions.Count();//var total = await query.CountAsync();
            // var totalPages = (int)Math.Ceiling(total * 1.0 / pageSize);

            // if (!string.IsNullOrEmpty(sortBy))
            //     if (sortOrder == SortOrder.Desc)
            //         transactions.OrderByDescending(sortBy, t=>t.Id);//query = query.OrderByDescending(sortBy, p => p.Id);
            //     else
            //         query = query.OrderBy(sortBy, p => p.Id);
            // else
            //     if (sortOrder == SortOrder.Desc)
            //         query = query.OrderByDescending(p => p.Id);
            //     else
            //         query = query.OrderBy(p => p.Id);

            // query = query.Skip((page - 1) * pageSize).Take(pageSize);

            // return new TransactionPagedList<TransactionEntity>
            // {
            //     Page = page,
            //     PageSize = pageSize,
            //     SortBy = sortBy,
            //     SortOrder = sortOrder,
            //     TotalCount = total,
            //     TotalPages = totalPages,
            //     Items = await query.ToListAsync(),
            // };





            return _mapper.Map<TransactionPagedList<Transaction>>(pagedSortedList);
        }
        public async Task<CategoryList> GetCategories(string parentId)
        {
            var categoriesList = await _pfmRepository.GetCategories();
            if (!string.IsNullOrEmpty(parentId))
                categoriesList = categoriesList.Where(c => parentId.Equals(c.ParentCode)).ToList();
            CategoryList categories = new CategoryList {
                Categories = _mapper.Map<List<Category>>(categoriesList)
            };
            return categories;
        }

        public async Task<List<Transaction>> ImportTransactions(IFormFile file)
        {
            string [] colFields;
            List<Transaction> ret = new List<Transaction>();

            using(TextFieldParser csvReader = new TextFieldParser(file.OpenReadStream()))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;
                colFields = csvReader.ReadFields();

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();

                    var check = await _pfmRepository.TransactionGet(fieldData[0]);
                    if (check != null)
                        continue;
                    
                    CreateTransactionCommand transaction = new CreateTransactionCommand();

                    transaction.Id = fieldData[0];
                    transaction.BeneficiaryName = fieldData[1];
                    transaction.Date = DateTime.Parse(fieldData[2]);
                    transaction.Direction = (Directions)Enum.Parse(typeof(Directions), fieldData[3], true);
                    transaction.Amount = Double.Parse(fieldData[4]);
                    transaction.Description = fieldData[5];
                    transaction.Currency = fieldData[6];
                    transaction.mcc = null;
                    transaction.Kind = (TransactionKind)Enum.Parse(typeof(TransactionKind), fieldData[8], true);

                    var createProduct = _mapper.Map<TransactionEntity>(transaction);
                    var productCreated = await _pfmRepository.CreateTransaction(createProduct);
                    ret.Add(_mapper.Map<Transaction>(productCreated));
                }
                return ret;
            }
        }

        public async Task<List<Category>> ImportCategories(IFormFile file)
        {
            string [] colFields;
            List<Category> ret = new List<Category>();

            using(TextFieldParser csvReader = new TextFieldParser(file.OpenReadStream()))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = false;
                colFields = csvReader.ReadFields();

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    
                    CreateCategoryCommand categoryCommand = new CreateCategoryCommand();

                    categoryCommand.Code = fieldData[0];
                    categoryCommand.ParentCode = fieldData[1];
                    categoryCommand.Name = fieldData[2];

                    CategoryEntity category = await _pfmRepository.CategoryGet(categoryCommand.Code);

                    if (category == null) 
                    {
                        CategoryEntity res = new CategoryEntity();
                        category = _mapper.Map<CategoryEntity>(categoryCommand);
                        if (string.IsNullOrEmpty(category.ParentCode))
                        {
                            category.ParentCode = null;
                            category.ParentCategory = null;
                            var check = await _pfmRepository.CategoryGet(category.ParentCode);
                            if (check != null)
                                res = await _pfmRepository.CreateCategory(category);
                        }
                        else 
                        {
                            var parentCategory = await _pfmRepository.CategoryGet(category.ParentCode);
                            if (parentCategory != null) {
                                category.ParentCategory = parentCategory;
                                if (parentCategory.ChildCategories == null)
                                    parentCategory.ChildCategories = new List<CategoryEntity>();
                                parentCategory.ChildCategories.Add(category);
                                res = await _pfmRepository.CreateCategory(category);
                                await _pfmRepository.UpdateCategory(parentCategory);
                            }
                        }
                        ret.Add(_mapper.Map<Category>(res));
                    }
                    else 
                    {
                        if (!string.IsNullOrEmpty(category.ParentCode))
                            category.ParentCode = categoryCommand.ParentCode;
                        category.Name = categoryCommand.Name;

                        await _pfmRepository.UpdateCategory(category);
                        ret.Add(_mapper.Map<Category>(category));
                    }
                }
            }
            return ret;
        }

        public async Task<Transaction> CategorizeTransaction(string id, TransactionCategorizeCommand command)
        {
            var transaction = await _pfmRepository.TransactionGet(id);;
            var category = await _pfmRepository.CategoryGet(command.CatCode);
            if (transaction != null && category != null)
            {
                transaction.CatCode = command.CatCode;
                transaction.Category = category;
                var res = await _pfmRepository.UpdateTransaction(transaction);
                return _mapper.Map<Transaction>(res);
            }
            return null;
        }

        public async Task<SpendingsByCategory> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, Directions? direction)
        {
            List<CategoryEntity> categories = await _pfmRepository.GetCategories();
            
            if (string.IsNullOrEmpty(catCode))
                categories = categories.Where(c=>string.IsNullOrEmpty(c.ParentCode)).ToList();
            else
                categories = categories.Where(c=>catCode.Equals(c.ParentCode)).ToList();
            List<SpendingInCategory> spendingsByCategory = new List<SpendingInCategory>();
            foreach (var category in categories) {
                List<TransactionEntity> transactions = await _pfmRepository.GetTransactions();
                transactions = transactions.Where(t=>category.Code.Equals(t.CatCode)).ToList();
                if (startDate != null)
                    transactions = transactions.Where(t=>t.Date >= startDate).ToList();
                if (endDate != null)
                    transactions = transactions.Where(t=>t.Date <= endDate).ToList();
                if (direction != null)
                    transactions = transactions.Where(t=>t.Direction.Equals(direction)).ToList();
                int count = transactions.Count();
                double? ammount = transactions.Sum(t=>t.Amount);
                spendingsByCategory.Add(new SpendingInCategory{
                    CatCode = category.Code,
                    Ammount = ammount,
                    Count = count
                });
            }
            return new SpendingsByCategory{
                Groups = spendingsByCategory
            };
        }
        public async Task<List<TransactionSplit>> SplitTransaction(string id, SplitTransactionCommand command)
        {
            var transaction = await _pfmRepository.TransactionGet(id);
            if (transaction != null)
            {
                double check = 0;
                foreach (SingleCategorySplit split in command.Splits) {
                    check += split.Amount;
                    var category = await _pfmRepository.CategoryGet(split.CatCode);
                    if (category == null)
                        return null;
                }
                if (check != transaction.Amount)
                    return null;
                List<TransactionSplit> res = new List<TransactionSplit>();
                var removed = await _pfmRepository.DeleteTransactionSplits(id);
                if (transaction.Splits != null)
                    transaction.Splits.Clear();
                foreach (SingleCategorySplit split in command.Splits)
                {
                    TransactionSplitEntity transactionSplit = _mapper.Map<TransactionSplitEntity>(split);
                    transactionSplit.TransactionId = id;
                    transactionSplit.Category = await _pfmRepository.CategoryGet(split.CatCode);
                    transactionSplit.ParentTransaction = await _pfmRepository.TransactionGet(id);
                    if (transaction.Splits == null)
                        transaction.Splits = new List<TransactionSplitEntity>();
                    transaction.Splits.Add(transactionSplit);
                    var pom = await _pfmRepository.CreateTransactionSplit(transactionSplit);
                    res.Add(_mapper.Map<TransactionSplit>(pom));
                }
                await _pfmRepository.UpdateTransaction(transaction);
                return res;
            }
            return null;
        }
    }
}