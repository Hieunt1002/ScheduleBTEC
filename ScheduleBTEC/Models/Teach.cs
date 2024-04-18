using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleBTEC.Models
{
    public class Teach
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeachId { get; set; }
        [Required]
        public string TeachName { get; set;}
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        public virtual Users Users { get; set; }
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Learn> Learns { get; set; } = new List<Learn>();
    }
}
