using Firebase.Auth;

namespace ShareCart.Interfaea;

public interface IAuthService {

    Task<UserCredential> LoginAsync(string email, string password);

    Task RegisterAsync(string email, string password);

    void LogoutAsync();

}
