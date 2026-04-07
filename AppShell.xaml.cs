using ShareCart.Views;

namespace ShareCart {

    public partial class AppShell : Shell {

        public AppShell() {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddItemToCartPageView), typeof(AddItemToCartPageView));

            Routing.RegisterRoute(nameof(UserProfilePageView), typeof(UserProfilePageView));

            Routing.RegisterRoute(nameof(ShoppingDetailsPageView), typeof(ShoppingDetailsPageView));
        }
    }
}
