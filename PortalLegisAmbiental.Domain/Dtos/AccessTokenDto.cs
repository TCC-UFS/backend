namespace PortalLegisAmbiental.Domain.Dtos
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; } = null!;
        public int ExpiresIn { get; set; } = 24 * 60 * 60; // 24h
    }
}
