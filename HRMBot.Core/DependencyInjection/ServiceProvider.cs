using HRMBot.Core.Security;
using Microsoft.EntityFrameworkCore;
using WEBASE.DependencyInjection;

namespace HRMBot.Core.DependencyInjection;

public class ServiceProvider : BaseServiceProvider<IAuthService>
{
    public static DbContext Context { get => GetService<DbContext>(); }
}
