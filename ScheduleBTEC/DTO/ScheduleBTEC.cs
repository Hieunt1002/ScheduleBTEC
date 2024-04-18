namespace ScheduleBTEC.DTO
{
    public class ViewScheduleDTO
    {
        public int ScheduleId { get; set; }
        public string coursename { get; set; }
        public string classname { get; set; }
        public DateTime DateLearn { get; set; }
        public string timelearn { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public bool status { get; set; }
        public string role { get; set; }
    }
}