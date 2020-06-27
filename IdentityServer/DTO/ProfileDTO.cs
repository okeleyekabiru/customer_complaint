
namespace IdentityServer.DTO
{
    public class ProfileDTO
    {
        public UserDto Type { get; set; }
        public string  ClientAccessToken { get; set; }
        public int ClientAccessTokenExpiry { get; set; }
        public string UserToken { get; set; }
        public int UserTokenExpiry { get; set; }
    }
}
