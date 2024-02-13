using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.DataLayer.EfClasses;

[Table("enum_caption_translate")]
public class CaptionTranslate : EnumTranslateEntity<CaptionTranslate, TranslateColumn>
{
    [ForeignKey(nameof(LanguageId))]
    public virtual Language Language { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Caption.Translates))]
    public virtual Caption Owner { get; set; }
}
