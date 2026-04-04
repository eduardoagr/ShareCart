using ShareCart.ViewModels;

namespace ShareCart.Views;

public partial class HomePageView : ContentPage {

    public HomePageView(HomePageViewModel pageViewModel) {
        InitializeComponent();

        BindingContext = pageViewModel;
    }
}