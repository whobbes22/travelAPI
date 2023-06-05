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
    public async Task<IActionResult> Put(int id, Review review)
    {
      if (id != review.ReviewId)
      {
        return BadRequest();
      }

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

    // GET: api/Reviews/5
    [HttpGet("popular")]
     public async Task<ActionResult<IEnumerable<Review>>> GetPopularReview()
    {
        string reviewDestination ="a place";
        // List<Review> userReviews = _db.Reviews.Where(entry => entry.ReviewDestination == reviewDestination).ToList();
        // List<Review> UserReviews = _db.Reviews.GroupBy(entry.ReviewDestination == reviewDestination).ToList();

/*
 var query = petsList.GroupBy(pet => Math.Floor(pet.Age),pet => pet.Age,
        (baseAge, ages) => new
        {
            Key = baseAge,
            Count = ages.Count(),
            Min = ages.Min(),
            Max = ages.Max()
        });

*/
        List<Review> userReviews = _db.Reviews.ToList();
        IEnumerable<IGrouping<string,int>> query = userReviews.GroupBy(review => review.ReviewDestination, review => review.ReviewRating);

        Dictionary<string, int> reviewGroup = new Dictionary<string,int>(){};
        foreach (IGrouping<string,int> destinationGroup in query)
        {

            reviewGroup[destinationGroup.Key] = destinationGroup.Count();
        }
        // var query = userReviews.GroupBy(entry =>entry.ReviewDestination, (something, somethingElse) => new
        // {
        //     Count = somethingElse.Count()
        // });
        // int count = -1;
        // foreach(var result in query)
        // {   
        //     if(count < result.Count)
        //     {
        //         count = result.Count;
        //     }
        // }
        Dictionary<string,int> dict = new Dictionary<string, int>();
        // make target dictionary
        // add list to dictionary, create destination and count value
            /*
            seattle: 3
            cali: 5
            portland: 2
            anotherplace: 1
            */
        // go through dictionary and find most reviews

        IQueryable<Review> query = _db.Reviews.AsQueryable();
        query = query.Where(entry => entry.ReviewDestination == reviewDestination);

        return await query.ToListAsync();

    //   Review review = await _db.Reviews.FindAsync();
    
    //   if (review == null)
    //   {
    //     return NotFound();
    //   }

    //   return review;





    }

  }
}
