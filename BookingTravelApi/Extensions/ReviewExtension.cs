// using BookingTravelApi.Domains;
// using BookingTravelApi.DTO.review;
// using BookingTravelApi.DTO.staff;

// namespace BookingTravelApi.Extensions
// {
//     public static class ReviewExtension
//     {
//         public static ReviewDTO Map(this Review review)
//         {
//             return new ReviewDTO
//             {
//                 Id = review.Id,
//                 Rating = review.Rating,
//                 Content = review.Content,
//                 CreatedAt = review.CreatedAt,
//                 CountHelpful = review.Helpfuls!.Count(),
//                 User = review.User!.Map(),
//                 Guides = review.Schedule!.Guides!.Select(i => i.Map()).ToList()
//             };
//         }
//     }
// }