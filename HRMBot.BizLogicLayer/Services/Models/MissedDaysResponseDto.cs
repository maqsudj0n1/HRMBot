using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.BizLogicLayer.Services.Models
{
    public class MissedDaysResponseDto
    {
        public int value { get; set; }
        public string text { get; set; }
        public object orderCode { get; set; }
    }
}
