using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Models;


namespace DatabaseLibrary.Database.InvoiceCouples;

public interface IInvoiceCoupleRepository
{
    Task CreateInvoiceMaterialCouple(int invoiceId, int materialId);
    Task CreateInvoiceServiceActionCouple(int invoiceId, int serviceActionId);

    Task<InvoiceMaterial?> GetInvoiceMaterialCouple(int invoiceId, int materialId);
    Task<List<InvoiceMaterial>?> GetAllInvoiceMaterialCouple(int invoiceId);
    Task<InvoiceServiceAction?> GetInvoiceServiceActionCouple(int invoiceId, int serviceActionId);
    Task<List<InvoiceServiceAction>?> GetAllInvoiceServiceActionCouple(int invoiceId);

    Task DeleteInvoiceMaterialCouple(int invoiceId, int materialId);
    Task DeleteInvoiceServiceActionCouple(int invoiceId, int serviceActionId);
}
