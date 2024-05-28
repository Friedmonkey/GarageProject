using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class InvoiceMaterial
{
    public int ID { get; set; }
    public Invoice Invoice { get; set; }
    public Material Material { get; set; }
    public float Amount { get; set; }
}
