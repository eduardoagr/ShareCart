using Firebase.Auth;

namespace ShareCart.Interfaces;

public interface IAuthService {

    Task<UserCredential> LoginAsync(string email, string password);

    Task<UserCredential> RegisterAsync(string email, string password);

    bool IsAuthenticated();

    string GetAuthUserID();

    string GetAuthUserEmail();

    void LogoutAsync();

}
