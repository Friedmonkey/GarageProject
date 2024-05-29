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
    Task CreateInvoiceServiceActionCouple(int invoiceId, int serviceActionId);
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
    Task<InvoiceServiceActionDTO?> GetInvoiceServiceActionCouple(int invoiceId, int serviceActionId);

    Task UpdateInvoice(int id,
        int? customerID = null,
        DateTime? date = null,
        List<InvoiceMaterial>? invoiceMaterials = null,
        List<InvoiceServiceAction>? serviceActions= null,
        float? serviceCost = null,
        float? AppointmentCost = null,
        string? brand = null
    );
    Task DeleteInvoice(int id);
    Task DeleteInvoiceMaterialCouple(int invoiceId, int materialId);
    Task DeleteInvoiceServiceActionCouple(int invoiceId, int serviceActionId);
}