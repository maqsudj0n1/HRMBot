using GenericServices;
using HRMBot.DataLayer.EfCode;
using HRMBot.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace HRMBot.DataLayer;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    public UnitOfWork(ICrudServices crudServices, IServiceProvider serviceProvider)
    {
        Context = (EfCoreContext)crudServices.Context;
        CrudServices = crudServices;
        _serviceProvider = serviceProvider;
    }
    public ICrudServices CrudServices { get; }
    public EfCoreContext Context { get; }
    public IDbContextTransaction CurrentTransaction { get => Context.Database.CurrentTransaction; }
    public TRepository GetRepository<TRepository>() => _serviceProvider.GetRequiredService<TRepository>();

    #region Repositories
    public IUserRepository UserRepository { get => GetRepository<IUserRepository>(); }
    #endregion

    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }
    public void Save()
    {
        Context.SaveChanges();
    }
    public void Commit()
    {
        Save();
        if (Context.Database.CurrentTransaction != null)
            Context.Database.CurrentTransaction.Commit();
    }
    public void Rollback()
    {
        if (Context.Database.CurrentTransaction != null)
            Context.Database.CurrentTransaction.Rollback();
    }
}