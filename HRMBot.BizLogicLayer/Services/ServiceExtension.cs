using HRMBot.BizLogicLayer.Services.Integration;
using HRMBot.BizLogicLayer.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.BizLogicLayer.Services
{
    public static class ServiceExtension
    {
        public static void ConfigureCrmServices(this IServiceCollection services, EmployeeManageConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<ICrmLoginService, CrmLoginService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }
    }
}
