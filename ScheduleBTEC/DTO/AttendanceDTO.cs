using ScheduleBTEC.Models;

namespace ScheduleBTEC.DTO
{
    public class AttendanceDTO
    {
        public Schedule Schedule { get; set; }
        public Learn Learn { get; set; }
        public Users Users { get; set; }
        public ClassEntity ClassEntity { get; set; }
        public Attendance Attendance { get; set; }
        public int UserId { get; set; }
        public bool status { get; set; }
    }
}
