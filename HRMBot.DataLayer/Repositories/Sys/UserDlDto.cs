using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE;
using HRMBot.DataLayer.EfClasses;

namespace HRMBot.DataLayer.Repositories
{
    public class UserDlDto<TDto> : EntityDto<TDto, User>
        where TDto : UserDlDto<TDto>
    {
        public int? LanguageId { get; set; }
        [LocalizedStringLength(50)]
        [LocalizedRequired]
        public string PhoneNumber { get; set; }
        public override User CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }

        public override void UpdateEntity(User entity)
        {
            base.UpdateEntity(entity);
        }

    }
}
