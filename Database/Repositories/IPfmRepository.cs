using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Models;

namespace pfm.Database.Repositories
{
    public interface IPfmRepository
    {
        Task<TransactionEntity> TransactionGet(string id);
        Task<CategoryEntity> CategoryGet(string code);
        Task<TransactionPagedList<TransactionEntity>> GetTransactions(List<TransactionKind> transactionKinds, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.asc);
        Task<List<TransactionEntity>> GetTransactions();
        Task<List<CategoryEntity>> GetCategories();
        Task<TransactionEntity> CreateTransaction(TransactionEntity transaction);
        Task<CategoryEntity> CreateCategory(CategoryEntity category);
        Task<CategoryEntity> UpdateCategory(CategoryEntity category);
        Task<TransactionEntity> UpdateTransaction(TransactionEntity transaction);
        Task<List<TransactionSplitEntity>> DeleteTransactionSplits(string id);
        Task<TransactionSplitEntity> CreateTransactionSplit(TransactionSplitEntity transactionSplit);
        Task<List<TransactionSplitEntity>> GetTransactionSplits();
    }
}