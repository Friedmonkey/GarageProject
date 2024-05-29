using DatabaseLibrary.Database.InvoiceCouples;
using DatabaseLibrary.Database.Materials;
using DatabaseLibrary.Database.ServiceActions;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Invoices;

public class InvoiceRepository : IInvoiceRepository
{
    //private readonly DatabaseContext _database;
    private readonly IDbContextFactory<DatabaseContext> _databaseFactory;

    private readonly IInvoiceCoupleRepository _invoiceCoupleRepository;
    public InvoiceRepository(IDbContextFactory<DatabaseContext> dbFactory, IInvoiceCoupleRepository invoiceCoupleRepository)
    {
        _databaseFactory = dbFactory;
        _invoiceCoupleRepository = invoiceCoupleRepository;
    }



    #region Helpers
    private async Task<Invoice> Resolve(InvoiceDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            UserAccount? customer = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.CustomerID);
            if (customer == null) return null; //throw new Exception("customer not found!");


            var materials = await _invoiceCoupleRepository.GetAllInvoiceMaterialCouple(entity.ID);
            var serviceActions = await _invoiceCoupleRepository.GetAllInvoiceServiceActionCouple(entity.ID);

            if (materials == null || serviceActions == null) return null;

            return new Invoice()
            {
                ID = entity.ID,
                Date = entity.Date,
                AppointmentCost = entity.AppointmentCost,
                Brand = entity.Brand,
                Customer = customer,
                Materials = materials,
                ServiceActions = serviceActions,
                ServiceCost = entity.ServiceCost,
            };
        }
    }

    private async Task<InvoiceDTO> Convert(Invoice entity)
    {
        return new InvoiceDTO()
        { 
            ID = entity.ID,
            Date = entity.Date,
            AppointmentCost = entity.AppointmentCost,
            Brand = entity.Brand,
            CustomerID = entity.Customer.ID,
            ServiceCost = entity.ServiceCost
        };
    }
    #endregion




    #region Create 
    public async Task<string> CreateInvoice(Invoice invoice)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            _database.Invoices.Add(await Convert(invoice));
            _database.SaveChanges();
            return "success";
        }
    }
    #endregion
    #region Read 
    public async Task<List<Invoice>> GetInvoicesByFilter(int? id = null, int? customerId = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            List<Invoice> invoices = new List<Invoice>();

            var querry = (await _database.Invoices.Where(a =>
                (id == null || a.ID == id) &&
                (customerId == null || a.CustomerID == customerId)
            ).ToListAsync());

            foreach (var invoice in querry)
            {
                var a = await Resolve(invoice);
                if (a != null)
                    invoices.Add(a);
            }
            return invoices;
        }
    }
    public async Task<Invoice?> GetInvoiceByFilter(int? id = null, int? customerId = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var invoiceDTO = (await _database.Invoices.Where(a =>
                (id == null || a.ID == id) &&
                (customerId == null || a.CustomerID == customerId)
            ).FirstOrDefaultAsync());

            if (invoiceDTO == null) return null;


            return await Resolve(invoiceDTO);
        }
    }
    //public async Task<List<Material>> GetMaterialsByInvoiceId(int invoiceID)
    //{
    //    using (var _database = _databaseFactory.CreateDbContext())
    //    {
    //        List<Material> materials = new List<Material>();

    //        //get all tournament-account couple from the tournament id
    //        var couples = _database.InvoiceMaterialCouples.Where(ta => ta.InvoiceId == invoiceID);

    //        if (couples != null)
    //        {
    //            foreach (var couple in couples)
    //            {
    //                //get the accociated useraccount from the couple we just got
    //                var material = _database.Materials.FirstOrDefault(a => a.ID == couple.MaterialId);
    //                if (material != null)
    //                    materials.Add(material);
    //            }
    //        }

    //        return materials;
    //    }
    //}
    //public async Task<List<InvoiceServiceAction>> GetServiceActionsByInvoiceId(int invoiceID)
    //{
    //    using (var _database = _databaseFactory.CreateDbContext())
    //    {
    //        List<InvoiceServiceAction> serviceActions = new List<InvoiceServiceAction>();

    //        //get all tournament-account couple from the tournament id
    //        var couples = _database.InvoiceServiceActionCouples.Where(ta => ta.InvoiceId == invoiceID);

    //        if (couples != null)
    //        {
    //            foreach (var couple in couples)
    //            {
    //                //get the accociated useraccount from the couple we just got
    //                var invoiceServiceActionDTO = _database.InvoiceServiceActionCouples.FirstOrDefault(a => a.ID == couple.ServiceActionId);
    //                if (invoiceServiceActionDTO != null)
    //                {
    //                    InvoiceServiceAction serviceAction = new ServiceAction() 
    //                    {
    //                        Name = invoiceServiceAction.
    //                    };
    //                    serviceActions.Add(invoiceServiceAction);
    //                }
    //            }
    //        }

    //        return serviceActions;
    //    }
    //}
    //public async Task<InvoiceMaterialDTO?> GetInvoiceMaterialCouple(int invoiceId, int materialId)
    //{
    //    using (var _database = _databaseFactory.CreateDbContext())
    //    {
    //        var couple = await _database.InvoiceMaterialCouples.FirstOrDefaultAsync(im => im.InvoiceId == invoiceId && im.MaterialId == materialId);
    //        if (couple != null)
    //        {
    //            return couple;
    //        }
    //        return null;
    //    }
    //}

    //public async Task<List<InvoiceMaterial>> GetAllInvoiceMaterialCouple(int invoiceId)
    //{
    //    using (var _database = _databaseFactory.CreateDbContext())
    //    {
    //        var couple = await _database.InvoiceMaterialCouples.Where(im => im.InvoiceId == invoiceId).ToListAsync();
    //        if (couple != null)
    //        {

    //            return await ConvertInvoiceMaterialDTOToInvoiceMaterial(couple);
    //        }
    //        return null;
    //    }
    //}

    #endregion
    #region Update 
    public async Task UpdateInvoice(int id, int? customerID = null, DateTime? date = null, float? serviceCost = null, float? AppointmentCost = null, string? brand = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = (await _database.Invoices.FirstOrDefaultAsync(a => a.ID == id));
            if (result != null)
            {
                if (customerID != null)
                    result.CustomerID = (int)customerID;
                if (date != null)
                    result.Date = (DateTime)date;
                if (serviceCost != null)
                    result.ServiceCost = (float)serviceCost;
                if (AppointmentCost != null)
                    result.AppointmentCost = (float)AppointmentCost;
                if (brand != null)
                    result.Brand = brand;

                _database.SaveChanges();
            }
        }
    }
    #endregion
    #region Delete 
    public async Task DeleteInvoice(int id)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = (await _database.Invoices.FirstOrDefaultAsync(a => a.ID == id));
            _database.Invoices.Remove(result);
            _database.SaveChanges();
        }
    }
    #endregion
}
