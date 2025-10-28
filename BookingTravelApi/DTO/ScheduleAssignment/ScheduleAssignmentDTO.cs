namespace BookingTravelApi.DTO.ScheduleAssignmentDTO
{
    public class ScheduleAssignmentDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool isAssignment { get; set; }
    }
}
