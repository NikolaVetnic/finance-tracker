namespace Application.Services.Abstractions;

public interface IServiceFactory
{
    ITokenService CreateTokenService();
}