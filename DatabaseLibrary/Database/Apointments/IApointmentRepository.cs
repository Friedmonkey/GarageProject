using DatabaseLibrary.Models;

namespace GarageProject.Services.Interfaces
{
    //Following the Crud syntax
    //Create
    //Read
    //Update
    //Delete
    public interface IApointmentRepository
    {
        Task<string> CreateAppointment(Apointment apointment);
        Task<List<Apointment>> GetApointmentsByFilter(
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
}