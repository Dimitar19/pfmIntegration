using AutoMapper;
using pfm.Models;
using pfm.Database.Entities;
using pfm.Commands;
using System.Collections.Generic;

namespace pfm.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionEntity, Transaction>();
            CreateMap<Transaction, TransactionEntity>();
            CreateMap<CreateTransactionCommand, TransactionEntity>();
            CreateMap<CreateTransactionCommand, Transaction>();

            CreateMap<TransactionPagedList<TransactionEntity>, TransactionPagedList<Transaction>>();

            CreateMap<CreateCategoryCommand, CategoryEntity>();
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<CategoryEntity, Category>();//.ForMember(x=>x.Code, opts=>opts.MapFrom(d=>d.Code));
            CreateMap<Category, CategoryEntity>();

            //CreateMap<List<CategoryEntity>, List<Category>>();

            CreateMap<SingleCategorySplit, TransactionSplitEntity>();
            CreateMap<TransactionSplitEntity, TransactionSplit>();

            //CreateMap<TransactionEntity, TransactionWithSplits>();
        }
    }
}