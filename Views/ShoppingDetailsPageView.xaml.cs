using ShareCart.Models;
using ShareCart.ViewModels;

namespace ShareCart.Views;

public partial class ShoppingDetailsPageView : ContentPage {

    public ShoppingDetailsPageView(ShoppingDetailsPageViewModel shoppingDetailsPageViewModel) {
        InitializeComponent();

        BindingContext = shoppingDetailsPageViewModel;

        shoppingDetailsPageViewModel.ScrollToEndRequested += OnScrollRequested;
    }

    private void OnScrollRequested() {

        if(BindingContext is ShoppingDetailsPageViewModel viewModel) {

            MainThread.BeginInvokeOnMainThread(async () => {

                await Task.Yield();

                var lastItem = viewModel.Products.LastOrDefault();

                if(lastItem != null) {
                    ProductList.ScrollTo(lastItem, null, ScrollToPosition.End, false);
                }
            });
        }
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();

        if(BindingContext is ShoppingDetailsPageViewModel viewModel) {
            viewModel.ScrollToEndRequested -= OnScrollRequested;
        }
    }
}