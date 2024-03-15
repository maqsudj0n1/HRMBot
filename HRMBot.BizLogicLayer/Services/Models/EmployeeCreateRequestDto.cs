using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.BizLogicLayer.Services.Models
{
    public class EmployeeCreateRequestDto
    {
        public int Id { get; set; }
        public long EmployeeManageId { get; set; }
        public int MissedDaysTypeId { get; set; }
        public int MissedDays { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool WithoutReason { get; set; }
        public string Details { get; set; }
    }
}
