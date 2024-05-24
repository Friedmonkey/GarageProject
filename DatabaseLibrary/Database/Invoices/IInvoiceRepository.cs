using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Models;

namespace DatabaseLibrary.Database.Invoices;

//Following the Crud syntax
//Create
//Read
//Update
//Delete
public interface IInvoiceRepository
{
    Task<string> CreateInvoice(Invoice invoice);
    Task CreateInvoiceMaterialCouple(int invoiceId, int materialId);
    Task<List<Invoice>> GetInvoicesByFilter(
        int? id = null,
        int? customerId = null
    );
    Task<Invoice> GetInvoiceByFilter(
        int? id = null,
        int? customerId = null
    );
    Task<List<Material>> GetMaterialsByInvoiceId(int invoiceID);
    Task<List<ServiceAction>> GetServiceActionsByInvoiceId(int invoiceID);
    Task<InvoiceMaterialDTO?> GetInvoiceMaterialCouple(int invoiceId, int materialId);

    Task UpdateInvoice(int id,
        int? customerID = null,
        DateTime? date = null,
        float? serviceCost = null,
        float? AppointmentCost = null,
        string? brand = null
    );
    Task DeleteInvoice(int id);
    Task DeleteInvoiceMaterialCouple(int invoiceId, int materialId);
}