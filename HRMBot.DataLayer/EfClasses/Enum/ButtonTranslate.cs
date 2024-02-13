using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.DataLayer.EfClasses;

[Table("enum_button_translate")]
public class ButtonTranslate : EnumTranslateEntity<ButtonTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Button.Translates))]
    public virtual Button Owner { get; set; }
}
