using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.BizLogicLayer.Services.Models
{
    public class CrmLoginResponseDto
    {
        public string token { get; set; }
        public string accessToken { get; set; }
        public string accessTokenExpireAt { get; set; }
        public string refreshToken { get; set; }
        public string refreshTokenExpireAt { get; set; }
        public UserInfo userInfo { get; set; }
    }

    public class UserInfo
    {
        public int id { get; set; }
        public object inn { get; set; }
        public string userName { get; set; }
        public object fullName { get; set; }
        public object shortName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public bool isAdmin { get; set; }
        public int languageId { get; set; }
        public string languageCode { get; set; }
        public string language { get; set; }
        public object pinfl { get; set; }
        public int organizationId { get; set; }
        public int positionId { get; set; }
        public int stateId { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string position { get; set; }
        public string organization { get; set; }
        public string organizationInn { get; set; }
        public string organizationVatCode { get; set; }
        public string organizationAddress { get; set; }
        public bool hasSecondUnitOfMeasure { get; set; }
        public int userTypeId { get; set; }
        public bool isSimpleUser { get; set; }
        public bool isOrgAdmin { get; set; }
        public bool isSuperAdmin { get; set; }
        public object userType { get; set; }
        public int employeeId { get; set; }
        public int organizationCurrencyId { get; set; }
        public List<int> userOrgAreasOfActivities { get; set; }
        public List<string> modules { get; set; }
        public List<string> roles { get; set; }
    }
}
