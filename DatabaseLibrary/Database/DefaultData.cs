using Bogus;
using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLibrary.Database
{
    public static class DefaultData
    {
        public static void Initialize(DatabaseContext db)
        {
            if (!db.Users.Any())
            {
                var faker = new Faker<UserAccount>()
                    .RuleFor(u => u.HashedPassword, f => $"{f.Name.FirstName()}|{f.Name.LastName()}")
                    .RuleFor(u => u.FullName, (f, u) => $"{u.Password.Split('|')[0]} {u.Password.Split('|')[1]}")
                    .RuleFor(u => u.Username, (f, u) => f.Internet.UserName(u.Password.Split('|')[0], u.Password.Split('|')[1]))
                    .RuleFor(u => u.ProfilePicture, (f, u) => f.Internet.Avatar())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username))
                    .RuleFor(u => u.Bio, f => f.Rant.Review("Garage").MakeGarage())
                    .RuleFor(u => u.Password, f => f.Internet.Password())
                    .RuleFor(u => u.Verified, true)
                    .RuleFor(u => u.Role, f => f.PickRandom(new[] { UserAccount.User, UserAccount.User, UserAccount.Secretary, UserAccount.Secretary, UserAccount.User, UserAccount.Mechanic, UserAccount.User }));

                // Generate default admin account
                var admin = new UserAccount
                {
                    FullName = "Admin",
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = "!Digilab2023!", // You may want to change this for production use
                    Verified = true,
                    Role = UserAccount.Admin
                };
                // make a list of all default accounts
                List<UserAccount> defaultAccounts = new List<UserAccount> { admin };

#if DEBUG
                // Generate default accounts using Bogus
                defaultAccounts.AddRange(faker.Generate(20));


                //change the default password to something easier
                admin.Password = "password";
#endif


                //add all default accounts to the database
                db.Users.AddRange(defaultAccounts);

                // Save changes to the database
                db.SaveChanges();
            }
        }
        public static string MakeGarage(this string input)
        {
            return input.Replace(" It ", " The garage ").Replace(" it ", " the garage ").Replace(" It", " The garage").Replace(" it", " the garage");
        }
    }
}
