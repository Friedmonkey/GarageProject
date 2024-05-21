using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Reviews;

public class ReviewRepository : IReviewRepository
{
    //private readonly DatabaseContext _database;
    private readonly IDbContextFactory<DatabaseContext> _databaseFactory;
    public ReviewRepository(IDbContextFactory<DatabaseContext> dbFactory)
    {
        _databaseFactory = dbFactory;
    }

    #region Helpers
    private async Task<Review> Resolve(ReviewDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            UserAccount? reviewer = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.ReviewerID);
            if (reviewer == null) return null; //throw new Exception("customer not found!");


            return new Review()
            {
                ID = entity.ID,
                Reviewer = reviewer,
                DatePosted = entity.DatePosted,
                ReviewStars = entity.ReviewStars,
                ReviewText = entity.ReviewText,
            };
        }
    }

    private async Task<ReviewDTO> Convert(Review entity)
    {
        using (var _database = _databaseFactory.CreateDbContext()) { }
        return new ReviewDTO()
        {
            ID = entity.ID,
            ReviewerID = entity.Reviewer.ID,
            DatePosted = entity.DatePosted,
            ReviewStars = entity.ReviewStars,  
            ReviewText = entity.ReviewText,
        };
    }
    #endregion

    #region Create 
    public async Task<string> CreateReview(Review review)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            if (await _database.Reviews.FirstOrDefaultAsync(a => a.ReviewerID == review.Reviewer.ID) == null)
            {
                _database.Reviews.Add(await Convert(review));
                _database.SaveChanges();
                return "success";
            }
            else return "You have already left an review!";
        }
    }
    #endregion
    #region Read 
    public async Task<List<Review>> GetReviewsByFilter(int? id = null, int? reviewerId = null, int? minReviewStars = null, int? maxReviewStars = null, string? reviewTextContains = null, DateTime? reviewPostedAfter = null, DateTime? reviewPostedBefore = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            List<Review> reviews = new List<Review>();

            var querry = (await _database.Reviews.Where(a =>
                (id == null || a.ID == id) &&
                (reviewerId == null || a.ReviewerID == reviewerId) &&
                (minReviewStars == null || a.ReviewStars >= (int)minReviewStars) &&
                (maxReviewStars == null || a.ReviewStars <= (int)maxReviewStars) &&

                (reviewPostedAfter == null || a.DatePosted >= (DateTime)reviewPostedAfter) &&
                (reviewPostedBefore == null || a.DatePosted <= (DateTime)reviewPostedBefore) &&

                (reviewTextContains == null || a.ReviewText.ToLower().Contains(reviewTextContains.ToLower()))
            ).ToListAsync());

            foreach (var review in querry)
            {
                var r = await Resolve(review);
                if (r != null)
                    reviews.Add(r);
            }
            return reviews;
        }
    }
    #endregion
    #region Update 
    //Reviews cannot be updated, you can delete them and make a new one instead
    #endregion
    #region Delete 
    public async Task DeleteReview(int id)
    {
        using (var _database = _databaseFactory.CreateDbContext()) 
        {
            var result = (await _database.Reviews.FirstOrDefaultAsync(a => a.ID == id));
            _database.Reviews.Remove(result);
            _database.SaveChanges();
        }
    }
    #endregion
}
