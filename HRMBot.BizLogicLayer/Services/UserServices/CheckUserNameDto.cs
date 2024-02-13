using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ihma_Support.BizLogicLayer.Services;

public class CheckUserNameDto
{
    public int? Id { get; set; }
    public string UserName { get; set; }
} 

public class CheckUserNameRespDto
{
    public string UserName { get; set; }
    public bool IsBusy { get; set; }
}
