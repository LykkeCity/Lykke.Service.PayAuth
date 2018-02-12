namespace Lykke.Service.PayAuth.Client.Models
{
    public class RegisterRequest
    {
        public string SystemId { get; set; }
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string Certificate { get; set; }
    }
}
