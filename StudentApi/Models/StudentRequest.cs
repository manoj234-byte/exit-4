namespace StudentApi.Models
{
    public class StudentRequest
    {
        public string? StudentName { get; set; }
        public DateTime DateTime { get; set; }
        public string? TargetTimeZone { get; set; }
    }
}
