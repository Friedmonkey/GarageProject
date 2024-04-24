using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Review
{
    public int Id { get; set; }
    public UserAccount Reviewer { get; set; }
    public string ReviewText { get; set; }
    public float ReviewStars { get; set; }


}
