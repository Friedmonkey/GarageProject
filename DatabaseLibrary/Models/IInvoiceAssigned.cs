using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public interface IInvoiceAssigned
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float Cost { get; set; }
}
