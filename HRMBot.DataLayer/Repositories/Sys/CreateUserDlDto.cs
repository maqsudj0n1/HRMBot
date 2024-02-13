using WEBASE.Attributes;

namespace HRMBot.DataLayer.Repositories
{
    public class CreateUserDlDto : UserDlDto<CreateUserDlDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string UserName { get; set; }
        public long ChatId { get; set; }
        public string FullName { get; set; }
        public int Step {  get; set; }
        public int StateId { get; set; }
    }
}
