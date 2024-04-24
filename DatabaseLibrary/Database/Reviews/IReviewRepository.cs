using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Reviews;

public interface IReviewRepository
{
    Task<string> CreateReview(Review review);
    Task<List<Review>> GetReviewsByFilter(
        int? id = null,
        int? reviewerId = null,

        float? minReviewStars = null,
        float? maxReviewStars = null,

        string? reviewTextContains = null,

        DateTime? reviewPostedAfter = null,
        DateTime? reviewPostedBefore = null
    );
    Task DeleteReview(int id);
}
