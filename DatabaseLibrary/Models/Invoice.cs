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
    public List<Material> Materials { get; set; }
    public List<ServiceAction> ServiceActions { get; set; }

    [NotMapped]
    public float MaterialCost => (Materials == null) ? 0 : Materials.Sum(m => m.TotalCost);

    [NotMapped]
    public float ServiceActionCost => (ServiceActions == null) ? 0 : ServiceActions.Sum(m => m.Price);


    [NotMapped]
    public float TotalCost => MaterialCost + ServiceActionCost + ServiceCost + AppointmentCost;
}
