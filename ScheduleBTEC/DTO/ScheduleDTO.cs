using ScheduleBTEC.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleBTEC.DTO
{
    public class ScheduleDTO
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int session { get; set; }
        public string timelearn { get; set; }
        public int LearnId { get; set; }
    }
}
