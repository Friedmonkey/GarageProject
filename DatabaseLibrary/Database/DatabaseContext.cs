using DatabaseLibrary.Database.Apointments;
using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Database.Reviews;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatabaseLibrary.Database;

public class DatabaseContext : DbContext
{
    public static string _defaultCconnectionString = "Dont change here, it will get overridden, change in appsettings instead";


    //public DatabaseContext()
    //{
    //    //Console.WriteLine("grabbing connection string");
    //    //var connection = _config.GetConnectionString("default");

    //    //if (connection == null)
    //    //    throw new Exception("\"default\" connection string is missing in appsettings!");

    //    //_defaultCconnectionString = connection;
    //}

    public DbSet<UserAccount> Users { get; set; }

    public DbSet<AppointmentDTO> Appointments { get; set; }

    public DbSet<InvoiceDTO> Invoices { get; set; }
    public DbSet<InvoiceMaterialDTO> InvoiceMaterialCouples { get; set; }
    public DbSet<InvoiceServiceActionDTO> InvoiceServiceActionCouples { get; set; }
    public DbSet<AppointmentServiceActionDTO> AppointmentServiceActionCouples { get; set; }

    public DbSet<Material> Materials { get; set; }
    public DbSet<ServiceAction> ServiceActions { get; set; }
    public DbSet<ReviewDTO> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_defaultCconnectionString);
    }
}
