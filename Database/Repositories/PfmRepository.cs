using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Models;

namespace pfm.Database.Repositories
{
    public class PfmRepository : IPfmRepository
    {
        private readonly PfmDbContext _dbContext;
        public PfmRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransactionEntity> TransactionGet(string id){
            var result = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id.Equals(id));
            return result;
        }
        public async Task<CategoryEntity> CategoryGet(string code){
            var result = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Code.Equals(code));
            return result;
        }
        public async Task<List<TransactionEntity>> GetTransactions(){
            return await _dbContext.Transactions.ToListAsync();
        }
        public async Task<List<CategoryEntity>> GetCategories(){
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<TransactionPagedList<TransactionEntity>> GetTransactions(List<TransactionKind> transactionKinds, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.asc)
        {
            var query = _dbContext.Transactions.AsQueryable();

            if(transactionKinds.Count > 0)
                query = query.Where(s => transactionKinds.Contains(s.Kind));
            if(startDate != null)
                query = query.Where(s=>s.Date >= startDate);
            if(endDate != null)
                query = query.Where(s=>s.Date <= endDate);

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total * 1.0 / pageSize);

            if (!string.IsNullOrEmpty(sortBy))
                if (sortOrder == SortOrder.desc)
                    query = query.OrderByDescending(sortBy, p => p.Id);
                else
                    query = query.OrderBy(sortBy, p => p.Id);
            else
                if (sortOrder == SortOrder.desc)
                    query = query.OrderByDescending(p => p.Id);
                else
                    query = query.OrderBy(p => p.Id);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            List<TransactionWithSplits> transactions = new List<TransactionWithSplits>();
            List<TransactionSplitEntity> transactionSplits = await GetTransactionSplits();
            foreach (TransactionEntity el in query)
            {
                var check = transactionSplits.Where(x => el.Id.Equals(x.TransactionId)).ToList();
                if (check != null && check.Count > 0)
                {
                    List<SingleCategorySplit> splits = new List<SingleCategorySplit>();
                    foreach (TransactionSplitEntity split in check)
                    {
                        splits.Add(new SingleCategorySplit{
                            CatCode = split.CatCode,
                            Amount = split.Amount
                        });
                    }
                    transactions.Add(new TransactionWithSplits{
                        Id = el.Id,
                        BeneficiaryName = el.BeneficiaryName,
                        Date = el.Date,
                        Direction = el.Direction,
                        Amount = el.Amount,
                        Description = el.Description,
                        Currency = el.Currency,
                        Mcc = el.Mcc,
                        Kind = el.Kind,
                        CatCode = el.CatCode,
                        Splits = splits.ToArray()
                    });
                }
                else 
                {
                    transactions.Add(new TransactionWithSplits{
                        Id = el.Id,
                        BeneficiaryName = el.BeneficiaryName,
                        Date = el.Date,
                        Direction = el.Direction,
                        Amount = el.Amount,
                        Description = el.Description,
                        Currency = el.Currency,
                        Mcc = el.Mcc,
                        Kind = el.Kind,
                        CatCode = el.CatCode,
                        Splits = new List<SingleCategorySplit>().ToArray()
                    });
                }
            }

            return new TransactionPagedList<TransactionEntity>
            {
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder,
                TotalCount = total,
                TotalPages = totalPages,
                Splits = transactions.ToArray()
            };
        }

        public async Task<TransactionEntity> CreateTransaction(TransactionEntity transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return await TransactionGet(transaction.Id);
        }
        public async Task<CategoryEntity> CreateCategory(CategoryEntity category){
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return await CategoryGet(category.Code);
        }

        public async Task<CategoryEntity> UpdateCategory(CategoryEntity category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return await CategoryGet(category.Code);
        }

        public async Task<TransactionEntity> UpdateTransaction(TransactionEntity transaction)
        {
            _dbContext.Transactions.Update(transaction);
            await _dbContext.SaveChangesAsync();
            return await TransactionGet(transaction.Id);
        }

        public async Task<List<TransactionSplitEntity>> DeleteTransactionSplits(string id)
        {
            List<TransactionSplitEntity> toRemove = await _dbContext.SplitTransactions.ToListAsync();
            toRemove = toRemove.Where(x => id.Equals(x.TransactionId)).ToList();
            _dbContext.SplitTransactions.RemoveRange(toRemove.ToArray());
            await _dbContext.SaveChangesAsync();
            return toRemove;
        }
        public async Task<TransactionSplitEntity> CreateTransactionSplit(TransactionSplitEntity transactionSplit)
        {
            await _dbContext.SplitTransactions.AddAsync(transactionSplit);
            await _dbContext.SaveChangesAsync();
            var res = await _dbContext.SplitTransactions.FirstOrDefaultAsync(x => x.Amount == transactionSplit.Amount && transactionSplit.CatCode.Equals(x.CatCode));
            return res;
        }
        public async Task<List<TransactionSplitEntity>> GetTransactionSplits()
        {
            return await _dbContext.SplitTransactions.ToListAsync();
        }
    }
}