using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace HRMBot.DataLayer.EfClasses;

[Table("enum_button")]
public partial class Button : IHaveIdProp<int>
{
    public Button()
    {
        Translates = new HashSet<ButtonTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string OrderCode { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(250)]
    public string FullName { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty(nameof(ButtonTranslate.Owner))]
    public virtual ICollection<ButtonTranslate> Translates { get; set; }
}

