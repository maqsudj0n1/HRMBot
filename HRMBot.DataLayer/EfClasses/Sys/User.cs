using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace HRMBot.DataLayer.EfClasses;
[Table("sys_user")]
public partial class User : IHaveIdProp<long>, IHaveStateId
{
    [Column("id")]
    public long Id { get; set; }
    [Column("user_name")]
    public string? UserName { get; set; }
    [Column("chat_id")]
    public long ChatId { get; set; }
    [Column("full_name")]
    public string? FullName { get; set; }
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }
    [Column("step")]
    public int Step { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("language_id")]
    public int? LanguageId { get; set; }
    [Column("start_date")]
    public DateTime? StartDate { get; set; }
    [Column("end_date")]
    public DateTime? EndDate { get; set; }
    [Column("selected_hour")]
    public int SelectedHour { get; set; }
    [Column("selected_minute")]
    public int SelectedMinute { get; set; }
    [Column("missed_day_type_id")]
    public int? MissedDayTypeId { get; set; }
    [Column("is_employee")]
    public bool IsEmployee { get; set; }

    [Column("last_access_time", TypeName = "timestamp without time zone")]
    public DateTime? LastAccessTime { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual Language? Language { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }


}
