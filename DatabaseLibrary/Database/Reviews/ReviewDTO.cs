using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Reviews;

public class ReviewDTO
{
    public int ID { get; set; }
    public int ReviewerID { get; set; }
    public DateTime DatePosted { get; set; }
    public string ReviewText { get; set; }
    public float ReviewStars { get; set; }
}
