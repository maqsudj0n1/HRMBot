using GenericServices.Configuration;
using AutoMapper;
using System.Linq.Dynamic.Core;
using HRMBot.BizLogicLayer.Services;
using HRMBot.DataLayer.EfClasses;
using HRMBot.DataLayer;
using WEBASE.DependencyInjection;

namespace HRMBot.Services
{
    public class UserDtoConfig : PerDtoConfig<UserDto, User>
    {
        public override Action<IMappingExpression<User, UserDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            ;
    }
}
