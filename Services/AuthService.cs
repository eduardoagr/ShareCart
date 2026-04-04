using Firebase.Auth;

using ShareCart.Interfaces;

namespace ShareCart.Services;

public class AuthService : IAuthService {

    private readonly IFirebaseAuthClient authClient;
    private readonly IUserRepoService userRepo;

    public AuthService(IFirebaseAuthClient firebaseAuthClient, IUserRepoService repoService) {

        authClient = firebaseAuthClient;
        userRepo = repoService;

        authClient.AuthStateChanged += AuthClient_AuthStateChanged;
    }

    private async void AuthClient_AuthStateChanged(object? sender, UserEventArgs e) {
        if(e.User != null) {
            await userRepo.UpdateLastLoginAsync(e.User.Uid);
        }
    }

    public async Task<UserCredential> LoginAsync(string email, string password)
        => await authClient.SignInWithEmailAndPasswordAsync(email, password);

    public async Task<UserCredential> RegisterAsync(string email, string password)
        => await authClient.CreateUserWithEmailAndPasswordAsync(email, password);

    public void LogoutAsync() => authClient.SignOut();

    public bool IsAuthenticated() => authClient.User != null;

    public string GetAuthUserID() => authClient.User.Uid;

    public string GetAuthUserEmail() => authClient.User.Info.Email;
}