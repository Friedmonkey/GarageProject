using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Models;

namespace DatabaseLibrary.Database.Apointments;

public class AppointmentServiceActionDTO
{
    public int ID { get; set; }
    public int AppointmentId { get; set; }
    public int ServiceActionId { get; set; }
}
