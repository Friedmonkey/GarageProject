using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Materials;

public interface IMaterialRepository
{
    Task<string> CreateMaterial(Material material);
    Task<List<Material>> GetMaterialsByFilter(
        int? id = null,
        int? customerId = null
    );
    Task UpdateMaterial(int id,
        int? customerID = null,
        DateTime? date = null,
        float? serviceCost = null,
        float? apointmentCost = null,
        string? brand = null
    );
    Task DeleteMaterial(int id);
}
