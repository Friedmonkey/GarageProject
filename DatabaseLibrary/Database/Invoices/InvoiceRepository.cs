using DatabaseLibrary.Models;
using GarageProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Invoices;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly DatabaseContext _database;

    public InvoiceRepository(DatabaseContext db)
    {
        _database = db;
    }



    #region Helpers
    private async Task<Invoice> Resolve(InvoiceDTO entity)
    {
        UserAccount? customer = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.CustomerID);
        if (customer == null) return null; //throw new Exception("customer not found!");


        List<Material> materials = await GetMaterialsByInvoiceId(entity.ID);

        return new Invoice()
        {
            ID = entity.ID,
            Date = entity.Date,
            ApointmentCost = entity.ApointmentCost,
            Brand = entity.Brand,
            Customer = customer,
            Materials = materials,
            ServiceCost = entity.ServiceCost,
        };
    }

    private async Task<InvoiceDTO> Convert(Invoice entity)
    {
        return new InvoiceDTO()
        { 
            ID = entity.ID,
            Date = entity.Date,
            ApointmentCost = entity.ApointmentCost,
            Brand = entity.Brand,
            CustomerID = entity.Customer.ID,
            ServiceCost = entity.ServiceCost
        };
    }
    #endregion




    #region Create 
    public async Task<string> CreateInvoice(Invoice invoice)
    {
        _database.Invoices.Add(await Convert(invoice));
        _database.SaveChanges();
        return "success";
    }
    public async Task CreateInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        await _database.InvoiceMaterialCouples.AddAsync(new InvoiceMaterialCouple()
        {
            InvoiceId = invoiceId,
            MaterialId = materialId
        });
        await _database.SaveChangesAsync();
    }
    #endregion
    #region Read 
    public async Task<List<Invoice>> GetInvoicesByFilter(int? id = null, int? customerId = null)
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
    public async Task<Invoice?> GetInvoiceByFilter(int? id = null, int? customerId = null)
    {
        var invoices = await GetInvoicesByFilter();
        if (invoices == null) return null;

        var invoice = invoices.FirstOrDefault();
        if (invoice == null) return null;

        return invoice;
    }
    public async Task<List<Material>> GetMaterialsByInvoiceId(int invoiceID)
    {
        List<Material> materials = new List<Material>();

        //get all tournament-account couple from the tournament id
        var couples = _database.InvoiceMaterialCouples.Where(ta => ta.InvoiceId == invoiceID);

        if (couples != null)
        { 
            foreach (var couple in couples)
            {
                //get the accociated useraccount from the couple we just got
                var material = _database.Materials.FirstOrDefault(a => a.ID == couple.MaterialId);
                if (material != null)
                    materials.Add(material);
            }
        }

        return materials;
    }
    public async Task<InvoiceMaterialCouple?> GetInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        var couple = await _database.InvoiceMaterialCouples.FirstOrDefaultAsync(im => im.InvoiceId == invoiceId && im.MaterialId == materialId);
        if (couple != null)
        {
            return couple;
        }
        return null;
    }
    #endregion
    #region Update 
    public async Task UpdateInvoice(int id, int? customerID = null, DateTime? date = null, float? serviceCost = null, float? apointmentCost = null, string? brand = null)
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
            if (apointmentCost != null)
                result.ApointmentCost = (float)apointmentCost;
            if (brand != null)
                result.Brand = brand;

            _database.SaveChanges();
        }
    }
    #endregion
    #region Delete 
    public async Task DeleteInvoice(int id)
    {
        var result = (await _database.Invoices.FirstOrDefaultAsync(a => a.ID == id));
        _database.Invoices.Remove(result);
        _database.SaveChanges();
    }
    public async Task DeleteInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        var couple = await GetInvoiceMaterialCouple(invoiceId, materialId);
        if (couple != null)
        {
            _database.InvoiceMaterialCouples.Remove(couple);
            await _database.SaveChangesAsync();
        }
    }
    #endregion
}
