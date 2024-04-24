using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Brand { get; set; }
    public List<Material> Materials { get; set; }
}
