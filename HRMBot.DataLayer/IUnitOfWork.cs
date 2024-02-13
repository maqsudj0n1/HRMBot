using HRMBot.DataLayer.EfCode;
using HRMBot.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRMBot.DataLayer;

public interface IUnitOfWork
{
    EfCoreContext Context { get; }
    IDbContextTransaction CurrentTransaction { get; }
    TRepository GetRepository<TRepository>();

    #region Repositories
    IUserRepository UserRepository { get; }


    #endregion

    IDbContextTransaction BeginTransaction();
    void Save();
    void Commit();
    void Rollback();
}
