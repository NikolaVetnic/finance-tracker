using Domain.Abstractions;

namespace Domain.Entities.Users.ValueObjects;

public class UserId(Guid value) : StronglyTypedId(value)
{
    public static UserId From(Guid value) => new(value);

    public static bool TryParse(string input, out UserId? userId)
    {
        if (Guid.TryParse(input, out var guid))
        {
            try
            {
                userId = new UserId(guid);
                return true;
            }
            catch (ArgumentException) { }
        }

        userId = null;
        return false;
    }

    public override string ToString() => Value.ToString();
}