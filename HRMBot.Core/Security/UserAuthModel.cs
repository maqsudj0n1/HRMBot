using Newtonsoft.Json;

namespace HRMBot.Core.Security;

public class UserAuthModel
{
    public int Id { get; set; }
    public string Inn { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public IEnumerable<string> Modules { get; set; } = new List<string>();
    public bool IsAdmin { get; set; }
    public int? LanguageId { get; set; }
    public string LanguageCode { get; set; }
    public string Pinfl { get; set; }
    public int OrganizationId { get; set; }
    public int? PositionId { get; set; }
    public bool? IsHr { get; set; }
    public void ResolveModules()
    {
        if (IsAdmin)
            Modules = Enum.GetNames(typeof(ModuleCode)).ToHashSet();
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

}
