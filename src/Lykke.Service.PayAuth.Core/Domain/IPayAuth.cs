namespace Lykke.Service.PayAuth.Core.Domain
{
    public interface IPayAuth
    {
        string SystemId { get; }
        string ClientId { get; }
        string Certificate { get; }
        string ApiKey { get; }
    }
}
