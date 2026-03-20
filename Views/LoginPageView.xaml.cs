using ShareCart.ViewModels;

namespace ShareCart.Views;

public partial class LoginPageView : ContentPage {
    public LoginPageView(LoginPageViewModel loginPageViewModel) {
        InitializeComponent();

        BindingContext = loginPageViewModel;
    }

}
