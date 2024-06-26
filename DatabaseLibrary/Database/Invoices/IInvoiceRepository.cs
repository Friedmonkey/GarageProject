﻿using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Models;

namespace DatabaseLibrary.Database.Invoices;

//Following the Crud syntax
//Create
//Read
//Update
//Delete
public interface IInvoiceRepository
{
    Task<int> CreateInvoice(Invoice invoice);
    Task<List<Invoice>> GetInvoicesByFilter(
        int? id = null,
        int? customerId = null
    );
    Task<Invoice> GetInvoiceByFilter(
        int? id = null,
        int? customerId = null
    );

    Task UpdateInvoice(int id,
        int? customerID = null,
        DateTime? date = null,
        float? serviceCost = null,
        float? AppointmentCost = null,
        string? brand = null
    );
    Task DeleteInvoice(int id);
}