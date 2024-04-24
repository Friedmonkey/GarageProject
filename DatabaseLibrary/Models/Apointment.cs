using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Apointment
{
    public int Id { get; set; }

    //the user who has requested the appointment
    // (send emails when appointment aproved or rejectec or when completed etcetera)
    public UserAccount ApointmentCreator { get; set; }
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public string Description { get; set; }
    public Invoice Invoice { get; set; }
    public UserAccount? MechanicAssigned { get; set; }
    public bool AprovedBySecretery { get; set; }
    public string SecreteryNote { get; set; }
}
