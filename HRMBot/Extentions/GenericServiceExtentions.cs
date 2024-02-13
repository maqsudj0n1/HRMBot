using GenericServices.Setup;
using HRMBot.DataLayer.EfCode;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace HRMBot.Extentions;

public static class GenericServiceExtentions
{
    public static void ConfigureGenericServices(this IServiceCollection services)
    {
        services.GenericServicesSimpleSetup<EfCoreContext>(
          //  Assembly.GetAssembly(typeof(UserDto)), //Service layer
            Assembly.GetAssembly(typeof(EfCoreContext)) //Data layer
        );

        services.RegisterAssemblyPublicNonGenericClasses(
            //Assembly.GetAssembly(typeof(UserDto)), //Service layer
            Assembly.GetAssembly(typeof(EfCoreContext)) //Data layer
        )
        .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
    }
}
