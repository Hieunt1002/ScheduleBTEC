using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleBTEC.Models
{
    public class Study
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudyId { get; set; }
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        public virtual Users Users { get; set; }
        [ForeignKey(nameof(Learn))]
        public int LearnId { get; set; }
        public virtual Learn Learn { get; set; }
    }
}
