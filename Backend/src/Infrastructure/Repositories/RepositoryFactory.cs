using Application.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public class RepositoryFactory(IServiceProvider serviceProvider) : IRepositoryFactory
{
    public IUsersRepository CreateUsersRepository()
    {
        return serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IUsersRepository>();
    }
}