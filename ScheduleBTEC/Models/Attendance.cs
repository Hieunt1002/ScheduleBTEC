using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleBTEC.Models
{
    public class Attendance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set;}
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        public virtual Users Users { get; set; }
        [ForeignKey(nameof(Schedule))]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }  
        public bool status { get; set; }
    }
}
