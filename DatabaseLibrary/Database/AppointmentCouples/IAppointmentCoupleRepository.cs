using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary.Database.Apointments;
using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Models;


namespace DatabaseLibrary.Database.AppointmentCouples;

public interface IAppointmentCoupleRepository
{
    Task CreateAppointmentServiceActionCouple(int appointmentId, int serviceActionId);

    Task<AppointmentServiceAction?> GetAppointmentServiceActionCouple(int appointmentId, int serviceActionId);
    Task<List<AppointmentServiceAction>?> GetAllAppointmentServiceActionCouple(int appointmentId);

    Task DeleteAppointmentServiceActionCouple(int appointmentServiceActionId);
}
