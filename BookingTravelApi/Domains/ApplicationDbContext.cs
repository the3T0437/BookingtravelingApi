using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Domains
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Favorite>().HasKey(i => new { i.TourId, i.UserId });
            modelBuilder.Entity<Helpful>().HasKey(i => new { i.ReviewId, i.UserId });
            modelBuilder.Entity<UserCompletedSchedule>().HasKey(i => new { i.ScheduleId, i.UserId });
            modelBuilder.Entity<Guide>().HasKey(i => new { i.ScheduleId, i.StaffId });
            modelBuilder.Entity<TourLocation>().HasKey(i => new { i.TourId, i.LocationId });
            modelBuilder.Entity<DayActivity>().HasKey(i => new { i.DayOfTourId, i.ActivityId, i.LocationActivityId });
            modelBuilder.Entity<ActivityAtLocationActivity>().HasKey(i => new { i.ActivityId, i.LocationActivityId});
        }
    }
}
