namespace Application.Data.Abstractions;

public interface IRepositoryFactory
{
    IUsersRepository CreateUsersRepository();
}