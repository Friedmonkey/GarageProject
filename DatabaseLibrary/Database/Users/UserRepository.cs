using DatabaseLibrary.Database;
using DatabaseLibrary.Models;
using GarageProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GarageProject.Services;

//Following the Crud syntax
//Create
//Read
//Update
//Delete
public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _database;

    public UserRepository(DatabaseContext db)
    {
        _database = db;
    }

    #region Create
    public async Task<string> Register(UserAccount account)
    {
        if (await _database.Users.FirstOrDefaultAsync(a => a.Username.ToLower() == account.Username.ToLower() || a.Email.ToLower() == account.Email.ToLower()) == null)
        {
            _database.Users.Add(account);
            _database.SaveChanges();
            return "success";
        }
        else if (await _database.Users.FirstOrDefaultAsync(a => a.Username.ToLower() == account.Username.ToLower()) != null)
            return $"Username \"{account.Username}\" is already taken.";
        else if (await _database.Users.FirstOrDefaultAsync(a => a.Email.ToLower() == account.Email.ToLower()) != null)
            return $"Email \"{account.Username}\" is already in use.";
        else return "unknown error";
    }
    #endregion

    #region Read
    public async Task<List<UserAccount>> GetUsersByFilter(
        int? id = null,
        string? username = null,
        string? fullName = null,
        string? role = null,
        string? email = null, 
        bool? verified = null
        )
    {
        return (await _database.Users.Where(a =>
            (id == null || a.Id == id) &&
            (username == null || a.Username.ToLower() == username.ToLower()) &&
            (fullName == null || a.FullName.Contains(fullName)) &&
            (role == null || a.Role == role) &&
            (email == null || a.Email.ToLower() == email.ToLower()) &&
            (verified == null || a.Verified == verified)
        ).ToListAsync());
    }
    public async Task<List<UserAccount>> GetUsersBySearchFilter(
        string? username = null, 
        string? fullName = null, 
        string? role = null, 
        string? email = null, 
        string? bio = null, 
        bool? verified = null
        )
    {
        return (await _database.Users.Where(a =>
            (
                (username == null || a.Username.ToLower().Contains(username.ToLower())) ||
                (fullName == null || a.FullName.ToLower().Contains(fullName.ToLower())) ||
                (role == null || a.Role.ToLower().Contains(role.ToLower())) ||
                (bio == null || a.Bio.ToLower().Contains(bio.ToLower())) ||
                (email == null || a.Email.ToLower().Contains(email.ToLower()))
            )
            &&
            (verified == null || a.Verified == (bool)verified)
        ).ToListAsync());
    }
    #endregion

    #region Update
    public async Task UpdateUser(int id, 
        string? username = null, 
        string? fullName = null, 
        string? role = null, 
        string? email = null, 
        string? address = null, 
        string? phone = null, 
        bool? banned = null, 

        string? password = null, 
        Guid? passwordResetGuid = null, 
        DateTime? passwordResetRequest = null, 

        bool? verified = null, 
        Guid? verifiedGuid = null,
        DateTime? verifyRequest = null
        )
    {

        var result = (await _database.Users.FirstOrDefaultAsync(a => a.Id == id));
        if (result != null)
        {
            if (username != null)
                result.Username = username;
            if (fullName != null)
                result.FullName = fullName;
            if (role != null)
                result.Role = role;
            if (email != null)
                result.Email = email;
            if (banned != null)
                result.Banned = (bool)banned;

            if (address != null)
                result.Addess = address;
            if (phone != null)
                result.PhoneNumber = phone;

            if (password != null)
                result.Password = password;

            if (passwordResetGuid != null)
                result.PasswordResetGuid = (Guid)passwordResetGuid;
            if (passwordResetRequest != null)
                result.PasswordResetRequest = (DateTime)passwordResetRequest;

            if (verified != null)
                result.Verified = (bool)verified;
            if (verifiedGuid != null)
                result.VerifyGuid = (Guid)verifiedGuid;
            if (verifyRequest != null)
                result.VerifyRequest = (DateTime)verifyRequest;

            _database.SaveChanges();
        }
    }
    public async Task UpdateUserByProperty(int id, string property, string value)
    {
        var result = await _database.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (result != null)
        {
            var propertyInfo = result.GetType().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(result, convertedValue);
                _database.SaveChanges();
            }
            else
            {
                // Handle property not found or not writeable
                // For instance: throw new Exception("Property not found or not writeable");
            }
        }
    }
    #endregion

    #region Delete
    public async Task DeleteUser(int id)
    {
        var result = (await _database.Users.FirstOrDefaultAsync(a => a.Id == id));
        _database.Users.Remove(result);
        _database.SaveChanges();
    }
    #endregion
}
