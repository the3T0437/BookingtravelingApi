using BookingTravelApi.Domains;
using BookingTravelApi.DTO.AssignmentDTO;


namespace BookingTravelApi.Extensions
{
    public static class AssignmentLocationExtention
    {
        public static AssignmentDTO MapAssignment(this Schedule schedule)
        {
            return new AssignmentDTO()
            {
                IdSchedule = schedule.Id,
                TitleTour = schedule.Tour!.Title,
                TourImages = schedule.Tour.TourImages?.Select(it => it.Path).ToList() ?? [],
                NameLocations = schedule.Tour.TourLocations?.Select(it => it.Location!.Name).ToList() ?? [],
                placeNames = schedule.Tour.TourLocations?.SelectMany(it => it.Location!.Places!).Select(p => p.Name).ToList() ?? [],
            };
        }
    }
}