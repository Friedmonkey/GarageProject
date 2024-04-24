using Bogus.DataSets;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Security.Principal;

namespace DatabaseLibrary.Database.Materials;

public class MaterialRepository : IMaterialRepository
{
    private readonly DatabaseContext _database;

    public MaterialRepository(DatabaseContext db)
    {
        _database = db;
    }

    #region Create 
    public async Task<string> CreateMaterial(Material material)
    {
        _database.Materials.Add(material);
        _database.SaveChanges();
        return "success";
    }
    #endregion
    #region Read 
    public async Task<List<Material>> GetMaterialsByFilter(int? id = null, string? name = null, float? singleCost = null, int? amount = null)
    {
        return (await _database.Materials.Where(a =>
            (id == null || a.ID == id) &&
            (name == null || a.Name.ToLower() == name.ToLower()) &&
            (singleCost == null || a.SingleCost == singleCost) &&
            (amount == null || a.Amount == amount)
        ).ToListAsync());
    }
    public async Task<List<Material>> GetMaterialsBySearchFilter(string? name = null)
    {
        return (await _database.Materials.Where(a => 
            (name == null || a.Name.ToLower().Contains(name.ToLower()))
        ).ToListAsync());
    }
    #endregion
    #region Update 
    public async Task UpdateMaterial(int id, string? name = null, float? singleCost = null, int? amount = null)
    {
        var result = (await _database.Materials.FirstOrDefaultAsync(a => a.ID == id));
        if (result != null)
        {
            if (name != null)
                result.Name = name;
            if (singleCost != null)
                result.SingleCost = (float)singleCost;
            if (amount != null)
                result.Amount = (int)amount;

            _database.SaveChanges();
        }
    }
    #endregion
    #region Delete 
    public async Task DeleteMaterial(int id)
    {
        var result = (await _database.Materials.FirstOrDefaultAsync(a => a.ID == id));
        _database.Materials.Remove(result);
        _database.SaveChanges();
    }
    #endregion
}