using Firebase.Auth;

using ShareCart.Interfaea;

namespace ShareCart.Services;

public class AuthService(FirebaseAuthClient authClient) : IAuthService {

    public async Task<UserCredential> LoginAsync(string email, string password) {

        var authUser = await authClient.SignInWithEmailAndPasswordAsync(email, password);

        return authUser;
    }

    public async Task RegisterAsync(string email, string password) {

        await authClient.CreateUserWithEmailAndPasswordAsync(email, password);
    }
    public void LogoutAsync() {
        authClient.SignOut();
    }
}
