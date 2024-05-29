using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class InvoiceMaterial : IInvoiceAssigned
{
    public int ID { get; set; }
    public Invoice Invoice { get; set; }
    public Material Material { get; set; }
    public float Amount { get; set; }
    public string Name { get => Material.Name; set => Material.Name = value; }
    public float Cost { get => Material.Cost; set => Material.Cost = value; }
}
