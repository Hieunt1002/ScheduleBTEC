using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleBTEC.Models
{
    public class ClassEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }
        [Required]
        public string className { get; set; }
        public virtual ICollection<Learn> Learns { get; set; } = new List<Learn>();
    }
}
