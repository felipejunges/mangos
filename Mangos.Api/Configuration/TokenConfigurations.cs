namespace Mangos.Api.Configuration
{
    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int TempoToken { get; set; }
        public int TempoRefreshToken { get; set; }
        public string Key { get; set; }

        public TokenConfigurations()
        {
            Audience = string.Empty;
            Issuer = string.Empty;
            Key = string.Empty;
        }
    }
}