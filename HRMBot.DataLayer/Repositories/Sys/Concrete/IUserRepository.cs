using HRMBot.DataLayer.EfClasses;
using HRMBot.DataLayer.Repositories;
using WEBASE.EF;

namespace HRMBot.DataLayer.Repositories
{
    public interface IUserRepository : IBaseEntityRepository<long, User, CreateUserDlDto, UpdateUserDlDto>
    {
    }
}
