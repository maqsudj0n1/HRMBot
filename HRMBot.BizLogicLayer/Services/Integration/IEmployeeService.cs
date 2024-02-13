using HRMBot.BizLogicLayer.Services.Models;
using StatusGeneric;
namespace HRMBot.BizLogicLayer.Services;

public interface IEmployeeService : IStatusGeneric
{
    Task<int> CheckEmployee(string phoneNumber);
    Task<List<UserModel>> GetUsers();

    Task<UserResponseData> CreateUser(UserRequestDto dto);

    Task<List<MissedDaysResponseDto>> MissedDaysTypeSelectList();
    Task<EmployeeCreateResponseDto> CreateByEmployee(EmployeeCreateRequestDto dto);

}
