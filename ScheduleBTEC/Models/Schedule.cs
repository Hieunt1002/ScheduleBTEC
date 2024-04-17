using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleBTEC.Models
{
    public class Schedule
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }
        [Required]
        public DateTime DateLearn { get; set; }
        [Required]
        public string timelearn { get; set; }
        [ForeignKey(nameof(Learn))]
        public int LearnId { get; set;}
        public virtual Learn Learn { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
