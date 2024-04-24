﻿using DatabaseLibrary.Models;

namespace GarageProject.Services.Interfaces
{
    //Following the Crud syntax
    //Create
    //Read
    //Update
    //Delete
    public interface IInvoiceRepository
    {
        Task<string> CreateInvoice(Invoice invoice);
        Task<List<Invoice>> GetInvoicesByFilter(
            int? id = null,
            int? customerId = null
        );
        Task UpdateInvoice(int id,
            int? customerID = null,
            DateTime? date = null,
            float? serviceCost = null,
            float? apointmentCost = null,
            string? brand = null
        );
        Task DeleteInvoice(int id);
    }
}