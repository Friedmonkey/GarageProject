using DatabaseLibrary.Models;

namespace GarageProject.Services.Interfaces
{
    //Following the Crud syntax
    //Create
    //Read
    //Update
    //Delete
    public interface IUserRepository
    {
        Task<string> Register(UserAccount account);
        Task<List<UserAccount>> GetUsersByFilter(int? id = null, string? username = null, string? fullName = null, string? role = null, string? email = null, bool? verified = null);
        Task<List<UserAccount>> GetUsersBySearchFilter(string? username = null, string? fullName = null, string? role = null, string? email = null, string? bio = null, bool? verified = null);
        Task UpdateUser(int id, string? username = null, string? fullName = null, string? role = null, string? email = null, bool? banned = null, string? password = null, Guid? passwordResetGuid = null, DateTime? passwordResetRequest = null, bool? verified = null, Guid? verifiedGuid = null, DateTime? verifyRequest = null);
        Task UpdateUserByProperty(int id, string property, string value);
        Task DeleteUser(int id);
    }
}