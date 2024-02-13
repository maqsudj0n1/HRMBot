

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.Models;

namespace HRMBot.DataLayer.EfClasses;

[Table("enum_state")]
public partial class State : IHaveIdProp<int>
{
    public State()
    {
        Translates = new HashSet<StateTranslate>();
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

    [InverseProperty(nameof(StateTranslate.Owner))]
    public virtual ICollection<StateTranslate> Translates { get; set; }

}
