using GenericServices;
using HRMBot.DataLayer.EfClasses;
using HRMBot.DataLayer.Repositories;
using WEBASE.EF;

namespace HRMBot.DataLayer.Repositories
{
    public class UserRepository : BaseEntityRepository<long, User, CreateUserDlDto, UpdateUserDlDto>, IUserRepository
    {
        public UserRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<User> ByIdQuery()
        {
            return base.ByIdQuery();
        }

        public User ByUserName(string userName)
        {
            var user = ByIdQuery().FirstOrDefault(a => a.UserName == userName);
            if (user == null)
            {
                AddEntityNotFoundError();
            }
            return user;
        }

        protected override void CreateValidate(CreateUserDlDto dto)
        {
            var query = DbSet.AsQueryable();
            if (query.Any(a => a.UserName == dto.UserName))
            {
                AddError("Это имя пользователя занято", dto.UserName);
            }
            Validate(null, dto);
        }

        protected override void UpdateValidate(User entity, UpdateUserDlDto dto)
        {
            var query = DbSet.AsQueryable();
            if (query.Any(a => a.Id != dto.Id && a.UserName == dto.UserName))
            {
                AddError("Это имя пользователя занято", dto.UserName);
            }
            Validate(entity, dto);
        }

        private void Validate<TDto>(User entity, UserDlDto<TDto> dto)
            where TDto : UserDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            
        }

        public void ClearErrors()
        {
            _errors?.Clear();
        }
    }
}
