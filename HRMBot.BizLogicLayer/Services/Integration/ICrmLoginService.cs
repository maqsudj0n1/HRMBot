using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.BizLogicLayer.Services.Integration
{
    public interface ICrmLoginService : IStatusGeneric
    {
        Task Login();
    }
}
