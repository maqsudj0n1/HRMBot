using HRMBot.Core.Security;
using HRMBot.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using WEBASE.i18n;

namespace HRMBot;

public class CultureHelper : BaseCultureHelper
{
    private readonly DbContext _context;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CultureHelper(CultureConfig config, DbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        : base(config)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override IEnumerable<CultureModel> GetCultures()
    {
        return _context.Set<Language>().AsEnumerable().Select(a => new CultureModel(a.Id, a.Code, a.FullName));
    }

    protected override CultureModel GetCurrentCulture()
    {
        if (_httpContextAccessor.HttpContext.Request.Query.TryGetValue("__lang", out StringValues lang) &&
            !StringValues.IsNullOrEmpty(lang) && Cultures.Any(a => a.Code == lang))
            return Cultures.First(a => a.Code == lang);
        if (_authService.IsAuthenticated)
            return Cultures.FirstOrDefault(a => a.Code == _authService.User.LanguageCode) ?? DefaultCulture;
        return DefaultCulture;
    }

    protected override CultureModel GetDefaultCulture()
    {
        return Cultures.FirstOrDefault(a => a.Code == "ru");
    }
}

