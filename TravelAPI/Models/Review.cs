namespace TravelAPI.Models
{
  public class Review
  {
    public int ReviewId { get; set; }
    public string ReviewDestination { get; set; }
    public string ReviewCountry { get; set; }
    public string ReviewUserName {get;set;}
    public int ReviewRating { get; set; }
  }
}