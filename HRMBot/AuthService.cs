using HRMBot.Core.Security;
using Microsoft.EntityFrameworkCore;
using WEBASE.AspNet.Security;
using WEBASE.EF;

namespace HRMBot;

public class AuthService : CookieJwtAuthService, IAuthService
{
    private readonly DbContext _context;

    public AuthService(IHttpContextAccessor httpContextAccessor,
                       DbContext dbContext,
    CookieConfig cookieConfig)
        : base(cookieConfig, httpContextAccessor)
    {
        _context = dbContext;
        ((BaseDbContext)_context).SetAuthService(this);
    }

    private UserAuthModel _user;
    public UserAuthModel User
    {
        get
        {


            return _user;
        }
    }

    private int? _userId;
    public override object UserId
    {
        get
        {
            if (_userId != null)
                return _userId;

            if (IsAuthenticated)
                _userId = this.User?.Id;

            return _userId;
        }
    }

    private HashSet<string> _modules;
    public override HashSet<string> Modules
    {
        get
        {
            if (IsAuthenticated && _modules == null)
                _modules = User.Modules.ToHashSet();
            return _modules;
        }
    }

    public bool HasPermission(params ModuleCode[] moduleCodes)
    {
        return moduleCodes.Any(a => HasPermission(a.ToString()));
    }

    private ContractorAuthModel? _contractor = null;
    public ContractorAuthModel Contractor
    {
        get
        {

            return _contractor;
        }
    }


    public OrganizationAuthModel _organization = null;
    public OrganizationAuthModel Organization
    {
        get
        {
            if (_organization == null)
            {
                var contractorIdString = HttpContextAccessor.HttpContext.Request.Cookies["organization-id"];

            }
            return _organization;
        }
    }

    protected override void Clear()
    {
        base.Clear();
        _user = null;
        _modules = null;
        _userId = null;
        _contractor = null;
    }

    public override void Logout()
    {
        HttpContextAccessor.HttpContext.Response.Cookies.Delete("contractor-id", new CookieOptions
        {
            HttpOnly = true,
            Domain = Config.Domain
        });
        base.Logout();
    }

    public void SelectContractor(long contractorId)
    {
        if (HttpContextAccessor.HttpContext.Request.Cookies.ContainsKey("contractor-id"))
        {
            HttpContextAccessor.HttpContext.Response.Cookies.Delete("contractor-id", new CookieOptions
            {
                HttpOnly = true,
                Domain = Config.Domain
            });
        }

        HttpContextAccessor.HttpContext.Response.Cookies.Append("contractor-id", $"{contractorId}", new CookieOptions
        {
            HttpOnly = true,
            Domain = Config.Domain,
            Expires = DateTimeOffset.Now.AddMinutes(Config.Expires)
        });

        // _contractor = _context.Set<Contractor>().Where(a => a.Id == contractorId).MapToAuthModel().FirstOrDefault();
    }
}