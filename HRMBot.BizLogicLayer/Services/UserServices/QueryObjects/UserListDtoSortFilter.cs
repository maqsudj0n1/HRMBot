using WEBASE.Models;
using System.Linq.Dynamic.Core;

namespace HRMBot.BizLogicLayer.Services
{
    public static class UserListDtoSortFilter
    {
        public static IQueryable<UserListDto> SortFilter(this IQueryable<UserListDto> query, ISortFilterOptions options)
        {
          
            query.ToList();
            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);
            return query;
        }
    }
}
