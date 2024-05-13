using DatabaseLibrary.Models;

namespace DatabaseLibrary.Database.Appointments;

//Following the Crud syntax
//Create
//Read
//Update
//Delete
public interface IAppointmentRepository
{
    Task<string> CreateAppointment(Appointment Appointment);
    Task<List<Appointment>> GetAppointmentsByFilter(
        int? id = null,
        int? creatorId = null,
        Status? status = null,
        int? mechanicId = null
    );
    Task UpdateAppointment(int id,
        int? creatorId = null,
        DateTime? date = null,
        Status? status = null,
        string? description = null,
        int? invoiceId = null,
        int? mechanicId = null,
        string? secreteryNote = null
    );
    Task DeleteAppointment(int id);
}