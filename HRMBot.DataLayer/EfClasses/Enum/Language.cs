using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace HRMBot.DataLayer.EfClasses;


[Table("enum_language")]
public partial class Language : IHaveIdProp<int>
{

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("code")]
    [StringLength(10)]
    public string Code { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(50)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

