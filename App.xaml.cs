using ShareCart.Interfaces;
using ShareCart.Views;

namespace ShareCart;


public partial class App : Application {

    private readonly IAuthService auth;
    private readonly IUserRepoService userRepoService;

    public App(IAuthService authService, IUserRepoService userRepo) {
        InitializeComponent();

        auth = authService;
        userRepoService = userRepo;
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        return new Window(new AppShell());
    }

    protected override async void OnStart() {
        base.OnStart();

        if(auth.IsAuthenticated()) {
            await Shell.Current.GoToAsync($"//{nameof(HomePageView)}", true);
        }
    }
}