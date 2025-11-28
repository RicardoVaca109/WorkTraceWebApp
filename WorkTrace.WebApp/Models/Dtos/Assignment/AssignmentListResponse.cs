namespace WorkTrace.WebApp.Models.Dtos.Assignment
{
    public class AssignmentListResponse
    {
        public string Id { get; set; }
        public string Client { get; set; }
        public string Service { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime AssignedTime { get; set; }
    }
}