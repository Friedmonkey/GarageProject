using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Invoices;

public class InvoiceDTO
{
    public int ID { get; set; }
    public int CustomerID { get; set; }
    public DateTime Date { get; set; }
    public float ServiceCost { get; set; }
    public float AppointmentCost { get; set; }
    public string Brand { get; set; }
}
