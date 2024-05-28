using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Invoice
{
    public int ID { get; set; }
    public UserAccount Customer { get; set; }
    public DateTime Date { get; set; }
    public float ServiceCost { get; set; }
    public float AppointmentCost { get; set; }
    public string Brand { get; set; }
    public List<InvoiceMaterial> Materials { get; set; }
    public List<InvoiceServiceAction> ServiceActions { get; set; }
    
}
