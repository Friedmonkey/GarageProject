using Bogus.DataSets;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Security.Principal;

namespace DatabaseLibrary.Database.Materials;

public class MaterialRepository : IMaterialRepository
{
    //private readonly DatabaseContext _database;
    private readonly IDbContextFactory<DatabaseContext> _databaseFactory;
    public MaterialRepository(IDbContextFactory<DatabaseContext> dbFactory)
    {
        _databaseFactory = dbFactory;
    }

    #region Create 
    public async Task<string> CreateMaterial(Material material)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            _database.Materials.Add(material);
            _database.SaveChanges();
            return "success";
        }
    }
    #endregion
    #region Read 
    public async Task<List<Material>> GetMaterialsByFilter(int? id = null, string? name = null, float? singleCost = null, int? amount = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            return (await _database.Materials.Where(a =>
                (id == null || a.ID == id) &&
                (name == null || a.Name.ToLower() == name.ToLower()) &&
                (singleCost == null || a.SingleCost == singleCost)
            ).ToListAsync());
        }
    }
    public async Task<List<Material>> GetMaterialsBySearchFilter(string? name = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            return (await _database.Materials.Where(a =>
                (name == null || a.Name.ToLower().Contains(name.ToLower()))
            ).ToListAsync());
        }
    }
    #endregion
    #region Update 
    public async Task UpdateMaterial(int id, Material m)
    {
        await UpdateMaterial(id, m.Name, m.SingleCost);
    }
    public async Task UpdateMaterial(int id, string? name = null, float? singleCost = null, int? amount = null)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = (await _database.Materials.FirstOrDefaultAsync(a => a.ID == id));
            if (result != null)
            {
                if (name != null)
                    result.Name = name;
                if (singleCost != null)
                    result.SingleCost = (float)singleCost;
                _database.SaveChanges();
            }
        }
    }
    #endregion
    #region Delete 
    public async Task DeleteMaterial(int id)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = (await _database.Materials.FirstOrDefaultAsync(a => a.ID == id));
            _database.Materials.Remove(result);
            _database.SaveChanges();
        }
    }
    #endregion
}