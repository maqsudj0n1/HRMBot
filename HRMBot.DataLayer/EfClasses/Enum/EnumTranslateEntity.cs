using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using HRMBot.Core.DependencyInjection;

namespace HRMBot.DataLayer.EfClasses;

public abstract class EnumTranslateEntity<TTranslate, TTranslateColumn> : EnumTranslateEntity<TTranslate, TTranslateColumn, int>
where TTranslate : EnumTranslateEntity<TTranslate, TTranslateColumn>
where TTranslateColumn : struct
{

}

public abstract class EnumTranslateEntity<TTranslate, TTranslateColumn, TOwnerId> : IHaveUniqueForeignKey, ITranslate
    where TTranslate : EnumTranslateEntity<TTranslate, TTranslateColumn, TOwnerId>
    where TTranslateColumn : struct
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public TOwnerId OwnerId { get; set; }
    [Column("language_id")]
    public int LanguageId { get; set; }
    [Required]
    [Column("column_name")]
    [StringLength(60)]
    public string ColumnName { get; set; }
    [Required]
    [Column("translate_text")]
    public string TranslateText { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    public object GetUniqueForeignKey() => $"{ColumnName}_{LanguageId}";

    public static Expression<Func<TTranslate, bool>> GetExpr(TTranslateColumn column, int? languageId)
    {
        string columnName = column.ToString();
        return a => a.LanguageId == languageId && a.ColumnName == columnName;
    }

    public static Expression<Func<TTranslate, bool>> GetExpr(TTranslateColumn column)
    {
        string columnName = column.ToString();
        return a => a.LanguageId == ServiceProvider.CultureHelper.CurrentCulture.Id && a.ColumnName == columnName;
    }
}

