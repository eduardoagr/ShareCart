using ShareCart.Models;
using ShareCart.ViewModels;

namespace ShareCart.Views;

public partial class AddItemToCartPageView : ContentPage {

    private bool _shareAdded = false;

    public AddItemToCartPageView(AddItemToCartPageViewModel pageViewModel) {
        InitializeComponent();

        BindingContext = pageViewModel;

        CreateStaticToolbarItems(pageViewModel);

        pageViewModel.ScrollToEndRequested += OnScrollRequested;

        pageViewModel.RequestAddShareButton += RequestAddShareButton;
    }

    private void RequestAddShareButton() {

        if(_shareAdded)
            return;

        _shareAdded = true;

        var vm = (AddItemToCartPageViewModel)BindingContext;

        var shareItem = new ToolbarItem {
            IconImageSource = new FontImageSource { Glyph = "\uE80D" },
            Command = vm.ControlPopUpStateCommand
        };

        ToolbarItems.Insert(0, shareItem);

    }

    private void CreateStaticToolbarItems(AddItemToCartPageViewModel pageViewModel) {

        var saveItem = new ToolbarItem {
            IconImageSource = new FontImageSource { Glyph = "\uE161" },
            Command = pageViewModel.SaveDatabaseCommand
        };

        ToolbarItems.Add(saveItem);

    }

    private void OnScrollRequested() {

        if(BindingContext is AddItemToCartPageViewModel viewModel) {
            MainThread.BeginInvokeOnMainThread(async () => {

                await Task.Yield();

                var lastItem = viewModel.Products.LastOrDefault();

                if(lastItem != null) {
                    ItemsList.ScrollTo(lastItem, null, ScrollToPosition.End, false);
                }
            });
        }
    }

    private async void ItemCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e) {

        if(sender is CheckBox cb && BindingContext is AddItemToCartPageViewModel vm &&
            cb.BindingContext is Product product) {

            await vm.CheckStatus(product, e.Value);
        }
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();

        if(BindingContext is AddItemToCartPageViewModel viewModel) {

            viewModel.ScrollToEndRequested -= OnScrollRequested;
            viewModel.RequestAddShareButton -= RequestAddShareButton;
        }
    }

    private void NewProductEntry_Loaded(object sender, EventArgs e) {

        if(sender is Entry entry && entry.BindingContext is Product product) {

            if(product.ShouldFocus) {

                product.ShouldFocus = false;

                MainThread.BeginInvokeOnMainThread(async () => {

                    await Task.Yield();

                    entry.Focus();
                });
            }
        }
    }
}