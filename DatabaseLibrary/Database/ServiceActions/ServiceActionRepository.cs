using Bogus.DataSets;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Security.Principal;
using System.Xml.Linq;

namespace DatabaseLibrary.Database.ServiceActions;

public class ServiceActionRepository : IServiceActionRepository
{
    //private readonly DatabaseContext _database;
    private readonly IDbContextFactory<DatabaseContext> _databaseFactory;
    public ServiceActionRepository(IDbContextFactory<DatabaseContext> dbFactory)
    {
        _databaseFactory = dbFactory;
    }

    #region Create 
    public async Task<string> CreateServiceAction(ServiceAction serviceAction)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            _database.ServiceActions.Add(serviceAction);
            _database.SaveChanges();
            return "success";
        }
    }
    #endregion
    #region Read 
    public async Task<List<ServiceAction>> GetServiceActionsByFilter(int? id = null, string? name = null, float? price = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            return (await _database.ServiceActions.Where(a =>
                (id == null || a.ID == id) &&
                (name == null || a.Name.ToLower() == name.ToLower()) &&
                (price == null || a.HourPrice.Equals(price))
            ).ToListAsync());
        }
    }

    public async Task<List<ServiceAction>> GetAllServiceActions()
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            return await _database.ServiceActions.ToListAsync();
        }
    }
    public async Task<List<ServiceAction>> GetServiceActionsBySearchFilter(string? name = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            return (await _database.ServiceActions.Where(a =>
                (name == null || a.Name.ToLower().Contains(name.ToLower()))
            ).ToListAsync());
        }
    }
    #endregion
    #region Update 
    public async Task UpdateServiceAction(int id, ServiceAction sa)
    {
        await UpdateServiceAction(id, sa.Name, sa.HourPrice, sa.Description);
    }
    public async Task UpdateServiceAction(int id, string? name = null, float? price = null, string? description = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = (await _database.ServiceActions.FirstOrDefaultAsync(a => a.ID == id));
            if (result != null)
            {
                if (name != null)
                    result.Name = name;
                if (price != null)
                    result.HourPrice = (float)price;
                if (description != null)
                    result.Description = (string)description;

                _database.SaveChanges();
            }
        }
    }
    #endregion
    #region Delete 
    public async Task DeleteServiceAction(int id)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = (await _database.ServiceActions.FirstOrDefaultAsync(a => a.ID == id));
            _database.ServiceActions.Remove(result);
            _database.SaveChanges();
        }
    }
    #endregion
}