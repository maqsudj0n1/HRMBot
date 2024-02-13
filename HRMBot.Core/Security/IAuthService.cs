using WEBASE.AspNet.Security;
namespace HRMBot.Core.Security;
public interface IAuthService : ICookieJwtAuthService
{
    UserAuthModel User { get; }
    OrganizationAuthModel Organization { get; }
    ContractorAuthModel Contractor { get; }
    bool HasPermission(params ModuleCode[] moduleCodes);
    void ResetUserName(string userName);
    void SelectContractor(long contractorId);
}
