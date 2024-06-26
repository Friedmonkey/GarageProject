﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class AppointmentDTO
{
    public int ID { get; set; }

    //the user who has requested the appointment
    // (send emails when appointment aproved or rejectec or when completed etcetera)
    public int AppointmentCreatorID { get; set; }
    public DateTime PlannedDate { get; set; }
    public DateTime CreationDate { get; set; }
    public Status Status { get; set; } = Status.Requested;
    public string Description { get; set; } = "";
    public int? InvoiceID { get; set; }
    public int? MechanicAssignedID { get; set; }
    public string? SecreteryNote { get; set; }
}
