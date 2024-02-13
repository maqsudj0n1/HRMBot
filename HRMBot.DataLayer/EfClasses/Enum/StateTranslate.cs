using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.DataLayer.EfClasses;

[Table("enum_state_translate")]
public partial class StateTranslate : EnumTranslateEntity<StateTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(State.Translates))]
    public virtual State Owner { get; set; }

    public static Expression<Func<StateTranslate, bool>> GetExpr(TranslateColumn full_name, object id)
    {
        throw new NotImplementedException();
    }
}
