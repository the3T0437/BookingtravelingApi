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

            modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .UseCollation("utf8mb4_bin");

            modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .UseCollation("utf8mb4_bin");

            CreatePrimaryKey(modelBuilder);
            CreateForeignKey(modelBuilder);
        }

        public DbSet<Configs> Configs => Set<Configs>();
        public DbSet<Actualcashs> Actualcashs => Set<Actualcashs>();
        public DbSet<Bank> Banks => Set<Bank>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<Status> Status => Set<Status>();
        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<TourImage> TourImages => Set<TourImage>();
        public DbSet<Tour> Tours => Set<Tour>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<UserCompletedSchedule> UserCompletedSchedules => Set<UserCompletedSchedule>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Helpful> Helpfuls => Set<Helpful>();
        public DbSet<DayOfTour> DayOfTours => Set<DayOfTour>();
        public DbSet<DayActivity> DayActivities => Set<DayActivity>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Guide> Guides => Set<Guide>();
        public DbSet<ActivityAndLocation> ActivityAndLocations => Set<ActivityAndLocation>();
        public DbSet<LocationActivity> LocationActivities => Set<LocationActivity>();
        public DbSet<Place> Places => Set<Place>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<ExternalImage> ExternalImages => Set<ExternalImage>();

        static private void CreatePrimaryKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>().HasKey(i => new { i.TourId, i.UserId });
            modelBuilder.Entity<Helpful>().HasKey(i => new { i.ReviewId, i.UserId });
            modelBuilder.Entity<UserCompletedSchedule>().HasKey(i => i.BookingId);
            modelBuilder.Entity<Guide>().HasKey(i => new { i.ScheduleId, i.StaffId });
            modelBuilder.Entity<DayActivity>().HasKey(i => new { i.DayOfTourId, i.ActivityId, i.LocationActivityId });
            modelBuilder.Entity<ActivityAndLocation>().HasKey(i => new { i.ActivityId, i.LocationActivityId });
        }

        static private void CreateForeignKey(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actualcashs>()
                .HasOne(a => a.Booking)
                .WithOne(b => b.Actualcashs)
                .HasForeignKey<Actualcashs>(actualcashs => actualcashs.BookingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Staff>()
                .HasOne(t => t.User)
                .WithOne(f => f.Staff)
                .HasForeignKey<Staff>(staff => staff.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(t => t.User)
                .WithMany(f => f.Notification)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Status)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.StatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Schedule)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ScheduleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TourImage>()
                .HasOne(t => t.Tour)
                .WithMany(f => f.TourImages)
                .HasForeignKey(t => t.TourId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(t => t.Tour)
                .WithMany(f => f.Favorites)
                .HasForeignKey(t => t.TourId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Favorite>()
                .HasOne(t => t.User)
                .WithMany(f => f.Favorites)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserCompletedSchedule>()
                .HasOne(u => u.Booking)
                .WithOne(b => b.UserCompletedSchedule)
                .HasForeignKey<UserCompletedSchedule>(u => u.BookingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasOne(t => t.Role)
                .WithMany(f => f.Users)
                .HasForeignKey(t => t.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Helpful>()
                .HasOne(t => t.User)
                .WithMany(f => f.Helpfuls)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Helpful>()
                .HasOne(t => t.Review)
                .WithMany(f => f.Helpfuls)
                .HasForeignKey(t => t.ReviewId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DayOfTour>()
                .HasOne(t => t.Tour)
                .WithMany(f => f.DayOfTours)
                .HasForeignKey(t => t.TourId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DayActivity>()
                .HasOne(t => t.DayOfTour)
                .WithMany(f => f.DayActivities)
                .HasForeignKey(t => t.DayOfTourId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DayActivity>()
                .HasOne(t => t.Activity)
                .WithMany(f => f.DayActivities)
                .HasForeignKey(t => t.ActivityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DayActivity>()
                .HasOne(t => t.LocationActivity)
                .WithMany(f => f.DayActivities)
                .HasForeignKey(t => t.LocationActivityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(t => t.Tour)
                .WithMany(f => f.Schedules)
                .HasForeignKey(f => f.TourId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(t => t.User)
                .WithMany(f => f.Reviews)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>()
                .HasOne(t => t.Schedule)
                .WithMany(f => f.Reviews)
                .HasForeignKey(t => t.ScheduleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guide>()
                .HasOne(t => t.Staff)
                .WithMany(f => f.Guides)
                .HasForeignKey(staff => staff.StaffId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Guide>()
                .HasOne(t => t.Schedule)
                .WithMany(f => f.Guides)
                .HasForeignKey(t => t.ScheduleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityAndLocation>()
                .HasOne(t => t.Activity)
                .WithMany(f => f.ActivityAndLocations)
                .HasForeignKey(t => t.ActivityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ActivityAndLocation>()
                .HasOne(t => t.LocationActivity)
                .WithMany(f => f.ActivityAndLocations)
                .HasForeignKey(t => t.LocationActivityId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LocationActivity>()
                .HasOne(t => t.Place)
                .WithMany(f => f.LocationActivities)
                .HasForeignKey(t => t.PlaceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Place>()
                .HasOne(t => t.Location)
                .WithMany(f => f.Places)
                .HasForeignKey(t => t.LocationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
