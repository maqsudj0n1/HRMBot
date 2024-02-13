using HRMBot.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.Models;

namespace HRMBot.DataLayer.Repositories
{
    public class UpdateUserDlDto : UserDlDto<UpdateUserDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
        public string UserName { get; set; }

        public override void UpdateEntity(User entity)
        {
            base.UpdateEntity(entity);
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                entity.UserName = UserName;
            }
        }
    }
}
