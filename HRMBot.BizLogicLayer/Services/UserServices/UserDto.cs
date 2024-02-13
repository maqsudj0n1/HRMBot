using GenericServices;
using HRMBot.DataLayer.EfClasses;
using HRMBot.DataLayer.Repositories;

namespace HRMBot.BizLogicLayer.Services
{
    public class UserDto : UpdateUserDlDto, ILinkToEntity<User>
    {
        public string Organization { get; set; }
        public string Region { get; set; }
        public bool IsOmbudsman { get; set; }
        public string State { get; set; }
      
    }
}
