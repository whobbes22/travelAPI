using Microsoft.EntityFrameworkCore;

namespace TravelAPI.Models
{
  public class TravelAPIContext : DbContext
  {
    public DbSet<Review> Reviews { get; set; }

    public TravelAPIContext(DbContextOptions<TravelAPIContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Review>()
        .HasData(
          new Review { ReviewId = 1, ReviewDestination = "Beach", ReviewCountry = "Space", ReviewUserName = "me", ReviewRating = 8 }
        );
    }
  }
}


    // public int ReviewId { get; set; }
    // public string ReviewDestination { get; set; }
    // public string ReviewCountry { get; set; }
    // public string ReviewUserName {get;set;}
    // public int ReviewRating { get; set; }