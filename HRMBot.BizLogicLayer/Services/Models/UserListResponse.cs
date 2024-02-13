namespace HRMBot.BizLogicLayer.Services.Models
{
    public class UserListResponse
    {
        public int status { get; set; }
        public string description { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public int count { get; set; }
        public List<UserModel> users { get; set; }
    }

    public class UserModel
    {
        public string id { get; set; }
        public string role_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string password { get; set; }
        public string user_type { get; set; }
        public DateTime created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class UserRequestDto
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public string phone_number { get; set; }
        public string role_id { get; set; }
    }
    public class UserResponseDto
    {
        public int status { get; set; }
        public string description { get; set; }
        public UserResponseData data { get; set; }
    }

    public class UserResponseData
    {
        public string id { get; set; }
        public string role_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string password { get; set; }
        public string user_type { get; set; }
        public DateTime created_at { get; set; }
        public string updated_at { get; set; }
    }
}
