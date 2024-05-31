using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class AppointmentServiceAction
{
    public int ID { get; set; }
    public int AppointmentId { get; set; }
    public ServiceAction ServiceAction { get; set; }
}
