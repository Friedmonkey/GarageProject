using Bogus.DataSets;
using DatabaseLibrary.Database.AppointmentCouples;
using DatabaseLibrary.Database.InvoiceCouples;
using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        //private readonly DatabaseContext _database;
        private readonly IDbContextFactory<DatabaseContext> _databaseFactory;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceCoupleRepository _invoiceCoupleRepository;
        private readonly IAppointmentCoupleRepository _appointmentCoupleRepository;
        public AppointmentRepository(IDbContextFactory<DatabaseContext> dbFactory, IInvoiceRepository invoiceRepository, IAppointmentCoupleRepository appointmentCoupleRepository, IInvoiceCoupleRepository invoiceCoupleRepository)
        {
            _databaseFactory = dbFactory;
            this._invoiceRepository = invoiceRepository;
            this._appointmentCoupleRepository = appointmentCoupleRepository;
            this._invoiceCoupleRepository = invoiceCoupleRepository;
        }

        #region Helpers
        private async Task<Appointment?> Resolve(AppointmentDTO entity) 
        {
            using (var _database = _databaseFactory.CreateDbContext()) 
            {
                UserAccount? AppointmentCreator = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.AppointmentCreatorID);
                if (AppointmentCreator == null) return null; //throw new Exception("Appointment creator not found!");

                Invoice? invoice = await _invoiceRepository.GetInvoiceByFilter(id: entity.InvoiceID);
                //if (invoice == null) return null; //throw new Exception("invoice not found!");

                UserAccount? mechanicAssigned = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.MechanicAssignedID);

                var appointmentServiceActionCouple = await _appointmentCoupleRepository.GetAllAppointmentServiceActionCouple(entity.ID);
                var defaultActions = appointmentServiceActionCouple.Select(asa => asa.ServiceAction).ToList();



                return new Appointment()
                {
                    ID = entity.ID,
                    CreationDate = entity.CreationDate,
                    PlannedDate = entity.PlannedDate,
                    AppointmentCreator = AppointmentCreator,
                    Description = entity.Description,
                    Invoice = invoice,
                    MechanicAssigned = mechanicAssigned,
                    SecreteryNote = entity.SecreteryNote,
                    DefaultActions = defaultActions,
                    Status = entity.Status
                };
            }
        }
        private async Task<AppointmentDTO> Convert(Appointment entity)
        {
            return new AppointmentDTO()
            {
                ID = entity.ID,
                CreationDate = entity.CreationDate,
                PlannedDate = entity.PlannedDate,
                AppointmentCreatorID = entity.AppointmentCreator.ID,
                Description = entity.Description,
                InvoiceID = entity?.Invoice?.ID,
                MechanicAssignedID = entity?.MechanicAssigned?.ID,
                SecreteryNote = entity?.SecreteryNote,
                Status = entity.Status
            };
        }
        #endregion

        #region Create 
        public async Task<string> CreateAppointment(Appointment Appointment)
        {
            using (var _database = _databaseFactory.CreateDbContext())
            {
                //if (await _database.Appointments.FirstOrDefaultAsync(a => a.PlannedDate == Appointment.PlannedDate) == null)
                //{
                var appointmentDTO = await Convert(Appointment);

                Invoice invoice = new Invoice(Appointment.AppointmentCreator);

                if (Appointment.Invoice != null)
                {
                    invoice = Appointment.Invoice;
                }
                int id = await _invoiceRepository.CreateInvoice(invoice);
                appointmentDTO.InvoiceID = id;

                foreach (var item in Appointment.DefaultActions)
                {
                    await _invoiceCoupleRepository.CreateInvoiceServiceActionCouple(id, item.ID, 1);
                }

                _database.Appointments.Add(appointmentDTO);

                foreach (var item in Appointment.DefaultActions)
                {
                    await _appointmentCoupleRepository.CreateAppointmentServiceActionCouple(appointmentDTO.ID, item.ID);
                }
                _database.SaveChanges();
                return "success";
                //}
                //else return "An appointment is already planned on this date";
            }
        }
        #endregion
        #region Read 
        public async Task<List<DateTime>> GetAllAppointmentsDatesByFilter(int? id = null, int? creatorId = null, Status? status = null, DateTime? afterdDate = null, DateTime? beforeDate = null, int? mechanicId = null)
        {
            using (var _database = _databaseFactory.CreateDbContext())
            {
                var querry = (await _database.Appointments.Where(a =>
                    (id == null || a.ID == id) &&
                    (creatorId == null || a.AppointmentCreatorID == creatorId) &&
                    (status == null || ((Status)status).HasFlag(a.Status)) &&
                    (beforeDate == null || a.PlannedDate < ((DateTime)beforeDate)) &&
                    (afterdDate == null || a.PlannedDate > ((DateTime)afterdDate)) &&
                    (mechanicId == null || a.MechanicAssignedID == mechanicId)
                ).Select(a => a.PlannedDate).ToListAsync());


                return querry;
            }
        }
        public async Task<List<Appointment>> GetAppointmentsByFilter(int? id = null, int? creatorId = null, Status? status = null, DateTime? afterdDate = null, DateTime? beforeDate = null, int? mechanicId = null)
        {
            using (var _database = _databaseFactory.CreateDbContext())
            {
                List<Appointment> Appointments = new List<Appointment>();

                var querry = (await _database.Appointments.Where(a =>
                    (id == null || a.ID == id) &&
                    (creatorId == null || a.AppointmentCreatorID == creatorId) &&
                    (status == null || ((Status)status).HasFlag(a.Status)) &&
                    (beforeDate == null || a.PlannedDate < ((DateTime)beforeDate)) &&
                    (afterdDate == null || a.PlannedDate > ((DateTime)afterdDate)) &&
                    (mechanicId == null || a.MechanicAssignedID == mechanicId)
                ).ToListAsync());

                foreach (var Appointment in querry)
                {
                    var a = await Resolve(Appointment);
                    if (a != null)
                        Appointments.Add(a);
                }
                return Appointments;
            }
        }
        #endregion
        #region Update 
        public async Task UpdateAppointment(int id, int? creatorId = null, DateTime? date = null, Status? status = null, string? description = null, int? invoiceId = null, int? mechanicId = null, string? secreteryNote = null)
        {
            using (var _database = _databaseFactory.CreateDbContext())
            {
                var result = (await _database.Appointments.FirstOrDefaultAsync(a => a.ID == id));
                if (result != null)
                {
                    if (creatorId != null)
                        result.AppointmentCreatorID = (int)creatorId;
                    if (date != null)
                        result.CreationDate = (DateTime)date;
                    if (status != null)
                        result.Status = (Status)status;
                    if (description != null)
                        result.Description = description;
                    if (invoiceId != null)
                        result.InvoiceID = (int)invoiceId;

                    if (mechanicId != null)
                        result.MechanicAssignedID = (int)mechanicId;
                    if (secreteryNote != null)
                        result.SecreteryNote = secreteryNote;

                    _database.SaveChanges();
                }
            }
        }
        #endregion
        #region Delete 
        public async Task DeleteAppointment(int id)
        {
            using (var _database = _databaseFactory.CreateDbContext()) 
            {
                var result = (await _database.Appointments.FirstOrDefaultAsync(a => a.ID == id));
                _database.Appointments.Remove(result);
                _database.SaveChanges();
            }
        }
        #endregion
    }
}
