using Bogus.DataSets;
using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Database.Materials;
using DatabaseLibrary.Database.ServiceActions;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DatabaseLibrary.Database.InvoiceCouples;

public class InvoiceCoupleRepository : IInvoiceCoupleRepository
{
    //private readonly DatabaseContext _database;
    private readonly IDbContextFactory<DatabaseContext> _databaseFactory;
    private readonly IMaterialRepository _materialRepository;
    private readonly IServiceActionRepository _serviceActionRepository;



    public InvoiceCoupleRepository(IDbContextFactory<DatabaseContext> dbFactory, IMaterialRepository materialRepository, IServiceActionRepository serviceActionRepository)
    {
        _databaseFactory = dbFactory;
        _materialRepository = materialRepository;
        _serviceActionRepository = serviceActionRepository;
    }

    #region Helpers
    private async Task<InvoiceMaterial> ResolveInvoiceMaterial(InvoiceMaterialDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var material = (await _materialRepository.GetMaterialsByFilter(id: entity.MaterialId)).FirstOrDefault();
            if (material == null)
            {
                return null;
            }

            var invoiceDTO = await _database.Invoices.FirstOrDefaultAsync(i => i.ID == entity.InvoiceId);
            if (invoiceDTO == null)
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
    private async Task<InvoiceServiceAction> ResolveInvoiceServiceAction(InvoiceServiceActionDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var invoiceDTO = await _database.Invoices.Where(i => i.ID == entity.InvoiceId).FirstOrDefaultAsync();
            if (invoiceDTO == null)
            {
                return null;
            }
            var serviceAction = (await _serviceActionRepository.GetServiceActionsByFilter(id: entity.ServiceActionId)).FirstOrDefault();
            if (serviceAction == null)
            {
                return null;
            }

            return new InvoiceServiceAction()
            {
                ID = entity.ID,
                InvoiceId = entity.InvoiceId,
                ServiceAction = serviceAction,
                Hours = entity.Hour,
                Name = serviceAction.Name,
                Cost = serviceAction.Cost,
            };
        }
    }
    private async Task<Invoice> ResolveInvoice(InvoiceDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            UserAccount? customer = await _database.Users.FirstOrDefaultAsync(u => u.ID == entity.CustomerID);
            if (customer == null) return null; //throw new Exception("customer not found!");


            var materials = await GetAllInvoiceMaterialCouple(entity.ID);
            var serviceActions = await GetAllInvoiceServiceActionCouple(entity.ID);

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
    #endregion


    #region Create
    public async Task CreateInvoiceMaterialCouple(int invoiceId, int materialId, float amount)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            await _database.InvoiceMaterialCouples.AddAsync(new InvoiceMaterialDTO()
            {
                InvoiceId = invoiceId,
                MaterialId = materialId,
                Amount = amount
            });
            await _database.SaveChangesAsync();
        }
    }

    public async Task CreateInvoiceServiceActionCouple(int invoiceId, int serviceActionId, float hour)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            await _database.InvoiceServiceActionCouples.AddAsync(new InvoiceServiceActionDTO()
            {
                InvoiceId = invoiceId,
                ServiceActionId = serviceActionId,
                Hour = hour
            });
            await _database.SaveChangesAsync();
        }
    }
    #endregion

    #region Read
    public async Task<InvoiceMaterial?> GetInvoiceMaterialCouple(int invoiceId, int materialId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = await _database.InvoiceMaterialCouples.FirstOrDefaultAsync(im => im.InvoiceId == invoiceId && im.MaterialId == materialId);

            if (result == null) return null;
            return await ResolveInvoiceMaterial(result);
        }
    }

    public async Task<List<InvoiceMaterial>?> GetAllInvoiceMaterialCouple(int invoiceId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            List<InvoiceMaterial> invoiceMaterials = new List<InvoiceMaterial>();

            var results = await _database.InvoiceMaterialCouples.Where(im => im.InvoiceId == invoiceId).ToListAsync();

            if (results == null) return null;

            foreach (var invoiceMaterial in results)
            {
                var a = await ResolveInvoiceMaterial(invoiceMaterial);
                if (a != null)
                    invoiceMaterials.Add(a);
            }
            return invoiceMaterials;
        }
    }

    public async Task<InvoiceServiceAction?> GetInvoiceServiceActionCouple(int invoiceId, int serviceActionId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = await _database.InvoiceServiceActionCouples.FirstOrDefaultAsync(im => im.InvoiceId == invoiceId && im.ServiceActionId == serviceActionId);

            if (result == null) return null;
            return await ResolveInvoiceServiceAction(result);
        }
    }

    public async Task<List<InvoiceServiceAction>?> GetAllInvoiceServiceActionCouple(int invoiceId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            List<InvoiceServiceAction> invoiceServiceActions = new List<InvoiceServiceAction>();

            var results = await _database.InvoiceServiceActionCouples.Where(im => im.InvoiceId == invoiceId).ToListAsync();

            if (results == null) return null;

            foreach (var invoiceServiceAction in results)
            {
                var a = await ResolveInvoiceServiceAction(invoiceServiceAction);
                if (a != null)
                    invoiceServiceActions.Add(a);
            }
            return invoiceServiceActions;
        }
    }
    #endregion

    #region Delete
    public async Task DeleteInvoiceMaterialCouple(int invoiceMaterialCoupleId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var couple = await _database.InvoiceMaterialCouples.FirstOrDefaultAsync(im => im.ID == invoiceMaterialCoupleId);
            if (couple != null)
            {
                _database.InvoiceMaterialCouples.Remove(couple);
                await _database.SaveChangesAsync();
            }
        }
    }

    public async Task DeleteInvoiceServiceActionCouple(int invoiceServiceActionId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var couple = await _database.InvoiceServiceActionCouples.FirstOrDefaultAsync(im => im.ID == invoiceServiceActionId);
            if (couple != null)
            {
                _database.InvoiceServiceActionCouples.Remove(couple);
                await _database.SaveChangesAsync();
            }
        }
    }
    #endregion
}
