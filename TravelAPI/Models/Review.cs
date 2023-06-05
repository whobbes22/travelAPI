using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
  public class Review
  {
    public int ReviewId { get; set; }
    [StringLength(255, ErrorMessage = "length can't exceed 255 characters")]
    public string ReviewDestination { get; set; }
    public string ReviewCountry { get; set; }
    [Required]
    public string ReviewUserName {get;set;}
    [Range(0,10, ErrorMessage ="Rating should be between 0 and 10")]
    public int ReviewRating { get; set; }
  }
}