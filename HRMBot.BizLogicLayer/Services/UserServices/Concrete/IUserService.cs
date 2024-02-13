using HRMBot.BizLogicLayer.Services;
using HRMBot.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace HRMBot.BizLogicLayer.Services
{
    public interface IUserService : IStatusGeneric
    {
        PagedResult<UserListDto> GetList(TableSortFilterPageOptions dto);
        UserDto Get();
        UserDto Get(int id);
        HaveId<long> Create(CreateUserDto dto);
        void Update(UpdateUserDlDto dto);
        void Delete(int id);
    }
}
