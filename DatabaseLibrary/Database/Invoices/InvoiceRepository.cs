using DatabaseLibrary.Models;
using GarageProject.Services.Interfaces;
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

    private async Task<Invoice> Resolve(InvoiceDTO entity)
    {

    }

    #region Create 
    public async Task<string> CreateInvoice(Invoice invoice)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Read 
    public async Task<List<Invoice>> GetInvoicesByFilter(int? id = null, int? customerId = null)
    {
        throw new NotImplementedException();
    }
    public async Task<Invoice?> GetInvoiceByFilter(int? id = null, int? customerId = null)
    {
        var invoices = await GetInvoicesByFilter();
        if (invoices == null) return null;

        var invoice = invoices.FirstOrDefault();
        if (invoice == null) return null;

        return invoice;
    }
    #endregion
    #region Update 
    public async Task UpdateInvoice(int id, int? customerID = null, DateTime? date = null, float? serviceCost = null, float? apointmentCost = null, string? brand = null)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Delete 
    public async Task DeleteInvoice(int id)
    {
        throw new NotImplementedException();
    }
    #endregion
}
