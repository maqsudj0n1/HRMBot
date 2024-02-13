using HRMBot.BizLogicLayer.Services.Integration;
using HRMBot.BizLogicLayer.Services.Models;
using Newtonsoft.Json;
using StatusGeneric;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using WEBASE.EF;
namespace HRMBot.BizLogicLayer.Services;

public class EmployeeService : StatusGenericHandler, IEmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly EmployeeManageConfig _config;
    private readonly ICrmLoginService _crmService;
    public EmployeeService(HttpClient httpClient, EmployeeManageConfig config, ICrmLoginService crmService)
    {
        _httpClient = httpClient;
        _config = config;
        _crmService = crmService;
    }

    public void Initialize(EmployeeManageConfig config)
    {
    }

    public async Task<int> CheckEmployee(string phoneNumber)
    {

        var dtoJson = JsonConvert.SerializeObject(phoneNumber, Formatting.Indented);

        var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"http://crmtest-api.apptest.uz/EmployeeManage/GetEmployeeManageId?phoneNumber={phoneNumber}", content);

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<int>(responseContent);

        return result;
    }
    public async Task<List<UserModel>> GetUsers()
    {
        var response = await _httpClient.GetAsync($"https://lms-vuny.onrender.com/user");

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<UserListResponse>(responseContent);

        return result.data.users;
    }
    public async Task<UserResponseData> CreateUser(UserRequestDto dto)
    {

        var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

        var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://lms-vuny.onrender.com/user", content);

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<UserResponseData>(responseContent);

        return result;
    }

    public async Task<List<MissedDaysResponseDto>> MissedDaysTypeSelectList()
    {
       await _crmService.Login();
        var url = $"{_config.Api}/hrm/Manual/MissedDaysTypeSelectList";
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Token);


        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            string error = $"CRM тизимидан маълумот олишда хатолик рўй берди!";
            try
            {
                error = await response.Content.ReadAsStringAsync();
            }
            catch { }

            AddError($"CRM тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
            return null!;
        }
        var responceJson = await response.Content.ReadAsStringAsync();
        List<MissedDaysResponseDto> data;
        try
        {
            data = JsonConvert.DeserializeObject<List<MissedDaysResponseDto>>(responceJson)!;

            if (data == null)
            {
                AddError($"CRM тизимидан маълумот топилмади!");
                return null!;
            }

            return data;
        }
        catch
        {
            throw;
        }
    }

    public async Task<EmployeeCreateResponseDto> CreateByEmployee(EmployeeCreateRequestDto dto)
    {
        await _crmService.Login();
        var url = $"{_config.Api}/hrm/EmployeeMissedDay/CreateByEmployee";
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Token);
        var dtoJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

        var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            string error = $"CRM тизимидан маълумот олишда хатолик рўй берди!";
            try
            {
                error = await response.Content.ReadAsStringAsync();
            }
            catch { }

            AddError($"CRM тизимидан маълумот олишда хатолик рўй берди!: {response.ReasonPhrase} error: {error}");
            return null;
        }
        var responceJson = await response.Content.ReadAsStringAsync();
        EmployeeCreateResponseDto res;
        try
        {
            res = JsonConvert.DeserializeObject<EmployeeCreateResponseDto>(responceJson)!;

            if (res == null)
            {
                AddError($"CRM тизимидан маълумот топилмади!");
                return  null;
            }

            return res;
        }
        catch
        {
            throw;
        }
    }

    
}
