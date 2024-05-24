using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class InvoiceServiceAction
{
    public int ID { get; set; }
    public int InvoiceId { get; set; }
    public ServiceAction ServiceAction { get; set; }
    public float Hours { get; set; }
}
