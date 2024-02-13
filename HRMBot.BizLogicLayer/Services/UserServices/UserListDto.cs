using GenericServices;
using HRMBot.DataLayer.EfClasses;
using WEBASE.Models;

namespace HRMBot.BizLogicLayer.Services
{
    public class UserListDto : ILinkToEntity<User>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
    }

}
