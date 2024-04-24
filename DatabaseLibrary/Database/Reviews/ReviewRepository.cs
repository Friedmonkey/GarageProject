using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Reviews;

public class ReviewRepository : IReviewRepository
{
    private readonly DatabaseContext _database;

    public ReviewRepository(DatabaseContext db)
    {
        _database = db;
    }

    private async Task<Review> Resolve(ReviewDTO entity)
    {

    }

    #region Create 
    public async Task<string> CreateReview(Review review)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Read 
    public async Task<List<Review>> GetReviewsByFilter(int? id = null, int? reviewerId = null, float? minReviewStars = null, float? maxReviewStars = null, string? reviewTextContains = null, DateTime? reviewPostedAfter = null, DateTime? reviewPostedBefore = null)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Update 
    //Reviews cannot be updated, you can delete them and make a new one instead
    #endregion
    #region Delete 
    public async Task DeleteReview(int id)
    {
        throw new NotImplementedException();
    }
    #endregion
}
