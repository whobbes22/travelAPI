using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;
using System.Linq;

namespace TravelAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReviewsController : ControllerBase
  {
    private readonly TravelAPIContext _db;

    public ReviewsController(TravelAPIContext db)
    {
      _db = db;
    }

    // GET api/reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get(string reviewDestination, string reviewCountry, int reviewRating, bool reviewExactRating)
    {
      IQueryable<Review> query = _db.Reviews.AsQueryable();
      if (reviewDestination != null)
      {
        query = query.Where(entry => entry.ReviewDestination == reviewDestination);
      }

      if (reviewCountry != null)
      {
        query = query.Where(entry => entry.ReviewCountry == reviewCountry);
      }

      if (reviewRating >= 0 && !reviewExactRating)
      {
        query = query.Where(entry => entry.ReviewRating >= reviewRating);
      }
      else 
      {
        query = query.Where(entry => entry.ReviewRating == reviewRating);
      }

      return await query.ToListAsync();
    }

    // GET: api/Reviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
      Review review = await _db.Reviews.FindAsync(id);

      if (review == null)
      {
        return NotFound();
      }

      return review;
    }

    // POST api/reviews
    [HttpPost]
    public async Task<ActionResult<Review>> Post(Review review)
    {
      _db.Reviews.Add(review);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
    }

    // PUT: api/Reviews/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Review review, string UserName)
    {

      //Review oldReview = await _db.Reviews.FindAsync(id);
      string oldUserName = review.ReviewUserName;
       
      if (id != review.ReviewId || oldUserName != UserName)
      {
        return BadRequest();
      }
      // if(oldUserName != UserName)
      // {
      //   return "bad user name";
      // }

      _db.Reviews.Update(review);

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ReviewExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    private bool ReviewExists(int id)
    {
      return _db.Reviews.Any(e => e.ReviewId == id);
    }

    // DELETE: api/Reviews/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
      Review review = await _db.Reviews.FindAsync(id);
      if (review == null)
      {
        return NotFound();
      }

      _db.Reviews.Remove(review);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    [HttpGet("popular")]
     public string GetPopularReview()
    {
      List<Review> userReviews = _db.Reviews.ToList();
      IEnumerable<IGrouping<string,int>> query = userReviews.GroupBy(review => review.ReviewDestination, review => review.ReviewRating);

      // Dictionary<string, int> reviewGroup = new Dictionary<string,int>(){};
      string bigRating = "";
      int maxCount = -1;
      foreach (IGrouping<string,int> destinationGroup in query)
      {
          if(maxCount < destinationGroup.Count())
          {
            maxCount = destinationGroup.Count();
            bigRating = destinationGroup.Count() + ": " + destinationGroup.Key;
          }
          else if( maxCount == destinationGroup.Count())
          {
            bigRating = bigRating + ", " + destinationGroup.Key; 
          }
          //reviewGroup[destinationGroup.Key] = destinationGroup.Count();
      }
      IQueryable<Review> queryPopular = _db.Reviews.AsQueryable();
      queryPopular = queryPopular.Where(entry => entry.ReviewDestination == bigRating);

      //return await queryPopular.ToListAsync();
      return bigRating;
    }

      // add all groups to a list
      // count the list
      // random number from 1 - counted list
      //return that index in the list to get all reviews from that destination
    [HttpGet("random")]
    public async Task<ActionResult<IEnumerable<Review>>> GetRandomDestination()
    {
      Random rand = new Random();

      List<Review> userReviews = _db.Reviews.ToList();
      IEnumerable<IGrouping<string,int>> query = userReviews.GroupBy(review => review.ReviewDestination, review => review.ReviewRating);

      List<string> randomDestinationList = new List<string>();
      foreach (IGrouping<string,int> destinationGroup in query)
      {
        randomDestinationList.Add(destinationGroup.Key);
      }
      int randNumber = rand.Next(0,randomDestinationList.Count());
      string randomDestination = randomDestinationList.ElementAt(randNumber);
    
      IQueryable<Review> queryRandom = _db.Reviews.AsQueryable();
      queryRandom = queryRandom.Where(entry => entry.ReviewDestination == randomDestination);
      return await queryRandom.ToListAsync();
    }
  }
}
