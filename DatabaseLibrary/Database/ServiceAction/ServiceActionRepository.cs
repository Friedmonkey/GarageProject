using Bogus.DataSets;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Security.Principal;

namespace DatabaseLibrary.Database.Actions;

public class ServiceActionRepository : IServiceActionRepository
{
    private readonly DatabaseContext _database;

    public ServiceActionRepository(DatabaseContext db)
    {
        _database = db;
    }

    #region Create 
    public async Task<string> CreateServiceAction(ServiceAction serviceAction)
    {
        _database.ServiceActions.Add(serviceAction);
        _database.SaveChanges();
        return "success";
    }
    #endregion
    #region Read 
    public async Task<List<ServiceAction>> GetServiceActionsByFilter(int? id = null, string? name = null, float? price = null)
    {
        return (await _database.ServiceActions.Where(a =>
            (id == null || a.ID == id) &&
            (name == null || a.Name.ToLower() == name.ToLower()) &&
            (price == null || a.Price == price)
        ).ToListAsync());
    }
    public async Task<List<ServiceAction>> GetServiceActionsBySearchFilter(string? name = null)
    {
        return (await _database.ServiceActions.Where(a => 
            (name == null || a.Name.ToLower().Contains(name.ToLower()))
        ).ToListAsync());
    }
    #endregion
    #region Update 
    public async Task UpdateServiceAction(int id, string? name = null, float? price = null, string? description = null)
    {
        var result = (await _database.ServiceActions.FirstOrDefaultAsync(a => a.ID == id));
        if (result != null)
        {
            if (name != null)
                result.Name = name;
            if (price != null)
                result.Price = (float)price;
            if (description != null)
                result.Description = (string)description;

            _database.SaveChanges();
        }
    }
    #endregion
    #region Delete 
    public async Task DeleteServiceAction(int id)
    {
        var result = (await _database.ServiceActions.FirstOrDefaultAsync(a => a.ID == id));
        _database.ServiceActions.Remove(result);
        _database.SaveChanges();
    }
    #endregion
}