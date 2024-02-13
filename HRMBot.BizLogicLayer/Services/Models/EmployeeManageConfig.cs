using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.BizLogicLayer.Services.Models
{
    public class EmployeeManageConfig
    {
        public string Api { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        internal string Token { get; set; }
    }
}
