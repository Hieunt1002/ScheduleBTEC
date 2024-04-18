using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ScheduleBTEC.Models
{
    public class Learn
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LearnId { get; set; }
        [Required]
        public string LearnName { get; set; }
        [ForeignKey(nameof(Teach))]
        public int TeachId { get; set; }
        public virtual Teach Teach { get; set; }

        [ForeignKey(nameof(ClassEntity))]
        public int ClassId { get; set; }
        public virtual ClassEntity ClassEntity { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Study> Study { get; set; } = new List<Study>();

    }
}