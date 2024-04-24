using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Material
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float SingleCost { get; set; }
    public int Amount { get; set; }

    [NotMapped]
    public float TotalCost => SingleCost * Amount;
}
