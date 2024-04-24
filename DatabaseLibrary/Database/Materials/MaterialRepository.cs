using DatabaseLibrary.Models;

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
        throw new NotImplementedException();
    }
    #endregion
    #region Read 
    public async Task<List<Material>> GetMaterialsByFilter(int? id = null, int? customerId = null)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Update 
    public async Task UpdateMaterial(int id, int? customerID = null, DateTime? date = null, float? serviceCost = null, float? apointmentCost = null, string? brand = null)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Delete 
    public async Task DeleteMaterial(int id)
    {
        throw new NotImplementedException();
    }
    #endregion
}