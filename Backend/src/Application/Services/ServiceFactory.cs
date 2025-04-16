using Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public class ServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
{
    public ITokenService CreateTokenService()
    {
        return serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITokenService>();
    }
}