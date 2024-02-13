using HRMBot.BizLogicLayer.Services.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StatusGeneric;
using System.Net.Http.Headers;
using System.Text;

namespace HRMBot.BizLogicLayer.Services.Integration
{
    public class CrmLoginService : StatusGenericHandler, ICrmLoginService
    {
        private readonly HttpClient _httpClient;
        private readonly EmployeeManageConfig _config;
        public CrmLoginService(EmployeeManageConfig config = null)
        {
            _httpClient = new HttpClient();
            _config = config;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
        }

        public async Task Login()
        {
            try
            {
                var loginDto = new LoginDto()
                {
                    UserName = _config.UserName,
                    Password = _config.Password,
                };
                var dtoJson = JsonConvert.SerializeObject(loginDto, Formatting.Indented);

                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
                var url = $"{_config.Api}/account/GenerateToken";

                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    string error = "";
                    try
                    {
                        error = await response.Content.ReadAsStringAsync();
                    }
                    catch { }

                    AddError($"CRM avtorizatsiyadan o'tishda xatolik!: {response.ReasonPhrase} error: {error}");
                    return;
                }
                try
                {
                    var responceJson = await response.Content.ReadAsStringAsync();
                    CrmLoginResponseDto res;
                    res = JsonConvert.DeserializeObject<CrmLoginResponseDto>(responceJson, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })!;
                    if (res.token == null)
                    {
                        AddError("CRM token olishda xatolik");
                    }
                    _config.Token = res.token;

                }
                catch
                {
                    throw;
                }

                return;
            }
            catch (Exception ex)
            {
                AddError($"CRM avtorizatsiyadan o'tishda xatolik! {ex.Message}: {ex.InnerException}");
            }
        }
    }
}
