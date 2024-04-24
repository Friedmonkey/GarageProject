using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Invoices;

public class InvoiceMaterialCouple
{
    public int ID { get; set; }
    public int InvoiceId { get; set; }
    public int MaterialId { get; set; }
}
