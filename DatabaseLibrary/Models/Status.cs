using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public enum Status
{
    Requested, //the appointment has been requested
    Rejected, //the appointment has been rejected
    Aproved, //the appointment has been aproved
    InProgress, //the appointment is in progress
    Completed, // the appointment is fully completed
}
