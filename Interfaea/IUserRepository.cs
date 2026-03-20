using ShareCart.Models;

namespace ShareCart.Interfaea;

public interface IUserRepository {

    Task SaveUserEmailAsync(string nodeName, User user);
}
