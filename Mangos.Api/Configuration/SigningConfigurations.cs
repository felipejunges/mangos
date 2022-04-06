using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mangos.Api.Configuration
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations(string keyString)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}