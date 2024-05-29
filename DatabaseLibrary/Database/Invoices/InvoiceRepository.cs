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
    private readonly IMaterialRepository _materialRepository;
    private readonly IServiceActionRepository _serviceActionRepository;


    public InvoiceRepository(IDbContextFactory<DatabaseContext> dbFactory, IMaterialRepository materialRepository, IServiceActionRepository serviceActionRepository)
    {
        _databaseFactory = dbFactory;
        _materialRepository = materialRepository;
        _serviceActionRepository = serviceActionRepository;
    }



    #region Helpers
    private async Task<Invoice> ResolveInvoice(InvoiceDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            UserAccount? customer = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.CustomerID);
            if (customer == null) return null; //throw new Exception("customer not found!");


            var materials = await GetAllInvoiceMaterialCouple(entity.ID);
            List<InvoiceServiceAction> serviceActions = await GetServiceActionsByInvoiceId(entity.ID);

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

    private async Task<InvoiceMaterial> ResolveInvoiceMaterial(InvoiceMaterialDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var invoiceDTO = await _database.Invoices.Where(i => i.ID == entity.InvoiceId).FirstOrDefaultAsync();
            if (invoiceDTO == null)
            {
                return null;
            }
            var material = (await _materialRepository.GetMaterialsByFilter(id: entity.MaterialId)).FirstOrDefault();
            if (material == null) 
            {
                return null;
            }

            return new InvoiceMaterial()
            {
                ID = entity.ID,
                Invoice = await ResolveInvoice(invoiceDTO),
                Material = material,
                Amount = entity.Amount,
            };
        }
    }
    private async Task<List<InvoiceMaterial>> ConvertInvoiceMaterialDTOToInvoiceMaterial(List<InvoiceMaterialDTO> invoiceMaterialDTOs)
    {
        List<InvoiceMaterial> returnList= new List<InvoiceMaterial>();

        foreach (var invoiceMaterialDTO in invoiceMaterialDTOs)
        {
            returnList.Add(await ResolveInvoiceMaterial(invoiceMaterialDTO));
        }
        return returnList;
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
    public async Task CreateInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            await _database.InvoiceMaterialCouples.AddAsync(new InvoiceMaterialDTO()
            {
                InvoiceId = invoiceId,
                MaterialId = materialId
            });
            await _database.SaveChangesAsync();
        }
    }
    public async Task CreateInvoiceServiceActionCouple(int invoiceId, int serviceActionId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            await _database.InvoiceServiceActionCouples.AddAsync(new InvoiceServiceActionDTO()
            {
                InvoiceId = invoiceId,
                ServiceActionId = serviceActionId
            });
            await _database.SaveChangesAsync();
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
                var a = await ResolveInvoice(invoice);
                if (a != null)
                    invoices.Add(a);
            }
            return invoices;
        }
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
        using (var _database = _databaseFactory.CreateDbContext())
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
    }
    public async Task<List<InvoiceServiceAction>> GetServiceActionsByInvoiceId(int invoiceID)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            List<InvoiceServiceAction> serviceActions = new List<InvoiceServiceAction>();

            //get all tournament-account couple from the tournament id
            var couples = _database.InvoiceServiceActionCouples.Where(ta => ta.InvoiceId == invoiceID);

            if (couples != null)
            {
                foreach (var couple in couples)
                {
                    //get the accociated useraccount from the couple we just got
                    var invoiceServiceActionDTO = _database.InvoiceServiceActionCouples.FirstOrDefault(a => a.ID == couple.ServiceActionId);
                    if (invoiceServiceActionDTO != null)
                    {
                        InvoiceServiceAction serviceAction = new ServiceAction() 
                        {
                            Name = invoiceServiceAction.
                        };
                        serviceActions.Add(invoiceServiceAction);
                    }
                }
            }

            return serviceActions;
        }
    }
    public async Task<InvoiceMaterialDTO?> GetInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var couple = await _database.InvoiceMaterialCouples.FirstOrDefaultAsync(im => im.InvoiceId == invoiceId && im.MaterialId == materialId);
            if (couple != null)
            {
                return couple;
            }
            return null;
        }
    }

    public async Task<List<InvoiceMaterial>> GetAllInvoiceMaterialCouple(int invoiceId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var couple = await _database.InvoiceMaterialCouples.Where(im => im.InvoiceId == invoiceId).ToListAsync();
            if (couple != null)
            {

                return await ConvertInvoiceMaterialDTOToInvoiceMaterial(couple);
            }
            return null;
        }
    }

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
    public async Task DeleteInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var couple = await GetInvoiceMaterialCouple(invoiceId, materialId);
            if (couple != null)
            {
                _database.InvoiceMaterialCouples.Remove(couple);
                await _database.SaveChangesAsync();
            }
        }
    }
    #endregion
}
