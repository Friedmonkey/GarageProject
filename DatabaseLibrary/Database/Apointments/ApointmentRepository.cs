using Bogus.DataSets;
using DatabaseLibrary.Models;
using GarageProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Apointments
{
    public class ApointmentRepository : IApointmentRepository
    {
        private readonly DatabaseContext _database;
        private readonly IInvoiceRepository _invoiceRepository;
        public ApointmentRepository(DatabaseContext db, IInvoiceRepository invoiceRepository)
        {
            _database = db;
            this._invoiceRepository = invoiceRepository;
        }

        #region Helpers
        private async Task<Apointment?> Resolve(ApointmentDTO entity) 
        {
            UserAccount? apointmentCreator = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.ApointmentCreatorID);
            if (apointmentCreator == null) return null; //throw new Exception("apointment creator not found!");

            Invoice? invoice = await _invoiceRepository.GetInvoiceByFilter(id: entity.InvoiceID);
            if (invoice == null) return null; //throw new Exception("invoice not found!");

            UserAccount? mechanicAssigned = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.MechanicAssignedID);

            return new Apointment()
            {
                ID = entity.ID,
                Date = entity.Date,
                ApointmentCreator = apointmentCreator,
                Description = entity.Description,
                Invoice = invoice,
                MechanicAssigned = mechanicAssigned,
                SecreteryNote = entity.SecreteryNote,
                Status = entity.Status
            };
        }
        private async Task<ApointmentDTO> Convert(Apointment entity)
        {
            return new ApointmentDTO()
            {
                ID = entity.ID,
                Date = entity.Date,
                ApointmentCreatorID = entity.ApointmentCreator.ID,
                Description = entity.Description,
                InvoiceID = entity.Invoice.ID,
                MechanicAssignedID = entity?.MechanicAssigned?.ID,
                SecreteryNote = entity.SecreteryNote,
                Status = entity.Status
            };
        }
        #endregion

        #region Create 
        public async Task<string> CreateAppointment(Apointment apointment)
        {
            if (await _database.Apointments.FirstOrDefaultAsync(a => a.Date == apointment.Date) == null)
            {
                _database.Apointments.Add(await Convert(apointment));
                _database.SaveChanges();
                return "success";
            }
            else return "An appointment is already planned on this date";
        }
        #endregion
        #region Read 
        public async Task<List<Apointment>> GetApointmentsByFilter(int? id = null, int? creatorId = null, Status? status = null, int? mechanicId = null)
        {
            List<Apointment> apointments = new List<Apointment>();

            var querry = (await _database.Apointments.Where(a =>
                (id == null || a.ID == id) &&
                (creatorId == null || a.ApointmentCreatorID == creatorId) &&
                (status == null || a.Status == status) &&
                (mechanicId == null || a.MechanicAssignedID == mechanicId)
            ).ToListAsync());

            foreach (var apointment in querry)
            {
                var a = await Resolve(apointment);
                if (a != null)
                    apointments.Add(a);
            }
            return apointments;
        }
        #endregion
        #region Update 
        public async Task UpdateAppointment(int id, int? creatorId = null, DateTime? date = null, Status? status = null, string? description = null, int? invoiceId = null, int? mechanicId = null, string? secreteryNote = null)
        {
            var result = (await _database.Apointments.FirstOrDefaultAsync(a => a.ID == id));
            if (result != null)
            {
                if (creatorId != null)
                    result.ApointmentCreatorID = (int)creatorId;
                if (date != null)
                    result.Date = (DateTime)date;
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
        #endregion
        #region Delete 
        public async Task DeleteAppointment(int id)
        {
            var result = (await _database.Apointments.FirstOrDefaultAsync(a => a.ID == id));
            _database.Apointments.Remove(result);
            _database.SaveChanges();
        }
        #endregion
    }
}
