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
    //MechanicFixed, //the appointment has been done on the mechanic's side, and its back for the HR to double check and finish
    AwaitingPayment, // the appointment is done but the user still has to pay
    Completed, // the appointment is fully completed


    Taken = Requested | Aproved | AwaitingPayment,
}
