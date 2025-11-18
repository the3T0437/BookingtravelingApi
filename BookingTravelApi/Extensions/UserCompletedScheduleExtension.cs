// using BookingTravelApi.Domains;
// using BookingTravelApi.DTO.usercompletedschedule;
// using BookingTravelApi.DTO.tourlocation;

// namespace BookingTravelApi.Extensions
// {
//     public static class UserCompletedScheduleExtension
//     {
//         public static UserCompletedScheduleDTO Map(this UserCompletedSchedule userCompletedSchedule)
//         {
//             return new UserCompletedScheduleDTO()
//             {
//                 Code = userCompletedSchedule.Schedule!.Code,
//                 StartDate = userCompletedSchedule.Schedule!.StartDate,
//                 EndDate = userCompletedSchedule.Schedule!.EndDate,
//                 Name = userCompletedSchedule.User!.Name,
//                 AvatarPath = userCompletedSchedule.User!.AvatarPath,
                
//                 // Loc userCompleted vs booking 
//                 Booking = userCompletedSchedule.Schedule!.Bookings!.Where(b => b.UserId == userCompletedSchedule.UserId).Select(b => b.Map()).ToList()
//             };
//         }
//     }
// }