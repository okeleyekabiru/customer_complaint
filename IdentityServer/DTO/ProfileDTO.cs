
using IdentityModel.Client;

namespace IdentityServer.DTO
{
    public class ProfileDTO
    {
        private UserDto User { get; set; }
        private string  ClientAccessToken { get; set; }
        private int ClientAccessTokenExpiry { get; set; }
        private string ClientScope { get; set; }
        public string UserScope { get; set; }
        public string UserToken { get; set; }
        public int UserTokenExpiry { get; set; }

        public ProfileDTO(UserDto user,TokenResponse client, TokenResponse useResponse)
        {
            User = user;
            ClientAccessToken = client.AccessToken;
            ClientAccessTokenExpiry = client.ExpiresIn;
            ClientScope = client.Scope;
            UserScope = useResponse.Scope;
            UserToken = useResponse.AccessToken;
            UserTokenExpiry = useResponse.ExpiresIn;
            



        }
    }
}
