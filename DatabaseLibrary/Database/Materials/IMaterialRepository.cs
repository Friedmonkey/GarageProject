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
        string? name = null,
        float? singleCost = null,
        int? amount = null
    );
    Task<List<Material>> GetMaterialsBySearchFilter(
        string? name = null
    );
    Task UpdateMaterial(int id,
        string? name = null,
        float? singleCost = null,
        int? amount = null
    );
    Task DeleteMaterial(int id);
}
