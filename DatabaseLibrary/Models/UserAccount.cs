using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Web;

namespace DatabaseLibrary.Models
{
    [DebuggerDisplay("{ID}:{Username}")]
    public class UserAccount
    {
        public UserAccount() { }
        public UserAccount(int id,string name, bool verified, string? password = null) 
        {
            ID = id;
            FullName = name;
            Username = name;
            Email = $"{name}@example.com";
            Verified = verified;
            if (password == null)
                Password = name;
            else
                Password = password;
        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string HashedPassword { get; set; }
        public string Addess { get; set; } = "Grove st.";
        public string PhoneNumber { get; set; } = "123456789";
        public string Role { get; set; } = UserAccount.User;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Bio { get; set; } = "Hello";
        public string ProfilePicture { get; set; } = "";
        public string Email { get; set; }

        public Guid PasswordResetGuid { get; set; }
        public DateTime PasswordResetRequest { get; set; } = DateTime.MinValue;

        public bool Verified { get; set; }
        public bool Banned { get; set; }
        public Guid VerifyGuid { get; set; }
        public DateTime VerifyRequest { get; set; } = DateTime.MinValue;

        [NotMapped]
        public const string ServerEmail = "info@Garage.nl";

        [NotMapped]
        private const string EmailUrl = "https://polydev.nl/Dev/Mail/request.php";

        [NotMapped]
        public const string Admin = "Administrator";

        [NotMapped]
        public const string Secretary = "Secretary";

        [NotMapped]
        public const string Mechanic = "Mechanic";

        [NotMapped]
        public const string User = "User";

        [NotMapped]
        public string Password 
        {
            get => HashedPassword;
            set => HashedPassword = Hash(value,Salt);
        }

        public static string Salt = "Dont change here, it will get overridden, change in appsettings instead";

        public async Task SendMail(string subject, string message) 
        {
            //string encode(string input) => NavigationMenu.Base64Encode(input);
            string encode(string input) => HttpUtility.UrlEncode(input);

            string url = $"{EmailUrl}?to={encode(Email)}&subject={encode(subject)}&message={encode(message)}&from={encode(ServerEmail)}";
            await Request(url);
        }

        private async Task<string> Request(string url)
        {
            HttpClient webClient = new HttpClient();
            try
            {
                string output = await webClient.GetStringAsync(url);
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "404 error";
            }
        }


        public static string Hash(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
    }
}
