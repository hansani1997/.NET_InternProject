using BlueLotus360.Com.Application.Exceptions;
using BlueLotus360.Com.Application.Specifications.Base;
using BlueLotus360.Com.Shared.Wrapper;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueLotus360.Com.Domain.Contracts;

namespace BlueLotus360.Com.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
        {
            await Task.CompletedTask;
            return null;
        }

        public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
        {
            return null;
        }
    }
}