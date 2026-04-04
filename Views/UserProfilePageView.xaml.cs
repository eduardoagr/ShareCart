using ShareCart.ViewModels;

namespace ShareCart.Views;

public partial class UserProfilePageView : ContentPage {

    public UserProfilePageView(UserProfilePageViewModel userProfilePageViewModel) {

        InitializeComponent();
        BindingContext = userProfilePageViewModel;
    }
}