using GarageProject.Shared;

namespace GarageProject.Auth
{
    public class UserSession
    {
        public string IdStr { get; set; }
        public string Username { get; set;}
        public string Role { get; set; }

        public int? ID => ParsingExtention.IntTryParse(IdStr);
    }
}
