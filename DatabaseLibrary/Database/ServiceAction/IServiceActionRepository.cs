using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Database.Actions;

public interface IServiceActionRepository
{
    Task<string> CreateServiceAction(ServiceAction serviceAction);
    Task<List<ServiceAction>> GetServiceActionsByFilter(
        int? id = null,
        string? name = null,
        float? price = null
    );
    Task<List<ServiceAction>> GetServiceActionsBySearchFilter(
        string? name = null
    );
    Task UpdateServiceAction(int id,
        string? name = null,
        float? price = null,
        string? description = null
    );
    Task DeleteServiceAction(int id);
}
