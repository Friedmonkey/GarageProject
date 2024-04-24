using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Review
{
    public int ID { get; set; }
    public UserAccount Reviewer { get; set; }
    public DateTime DatePosted { get; set; }
    public string ReviewText { get; set; }
    public int ReviewStars { get; set; }
}
