using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using LocalizationResourceManager.Maui;

using ShareCart.Interfaces;
using ShareCart.Models;
using ShareCart.Views;

using System.Collections.ObjectModel;

namespace ShareCart.ViewModels;

public partial class HomePageViewModel : ObservableObject {

    private readonly IShoppingListService listService;
    private readonly IAuthService authService;
    private readonly IMessenger messenger;
    private readonly ILocalizationResourceManager localizationResource;

    private bool _isLoaded = false;

    public ObservableCollection<ShoppingList> ShoppingLists { get; set; } = [];


    public HomePageViewModel(IShoppingListService listService, IAuthService authService,
        IMessenger messenger, ILocalizationResourceManager localization) {

        this.listService = listService;
        this.authService = authService;
        this.messenger = messenger;
        localizationResource = localization;

        messenger.Register<ShoppingList>(this, (r, newItem) => {

            newItem.IsMine = newItem.OwnerId == authService.GetAuthUserID();

            if(!ShoppingLists.Any(x => x.Id == newItem.Id))
                ShoppingLists.Add(newItem);
        });
    }

    [RelayCommand]
    void CreateNewList() {

        Shell.Current.GoToAsync(nameof(AddItemToCartPageView), true);
    }

    [RelayCommand]
    async Task PageAppearing() {

        if(_isLoaded)
            return;

        _isLoaded = true;


        var userId = authService.GetAuthUserID();

        if(string.IsNullOrWhiteSpace(userId))
            return;


        ShoppingLists.Clear();

        var list = await listService.GetShoppingListAsync(authService.GetAuthUserID());

        foreach(var item in list) {

            item.IsMine = item.OwnerId == authService.GetAuthUserID();

            ShoppingLists.Add(item);
        }

    }

    [RelayCommand]
    async Task EditProfile() {

        await Shell.Current.GoToAsync(nameof(UserProfilePageView));
    }

    [RelayCommand]
    async Task OpenList(ShoppingList list) {
        var parameters = new Dictionary<string, object> {
            { "list", list }
        };
        await Shell.Current.GoToAsync(nameof(AddItemToCartPageView), parameters);
    }

    [RelayCommand]
    async Task LongPress(ShoppingList list) {

        if(!list.IsMine) {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync(
            localizationResource["UI_DeleteConfirmTitle"],
            localizationResource["UI_DeleteConfirmMessage"],
            localizationResource["UI_Yes"],
            localizationResource["UI_No"]
        );

        if(!confirm)
            return;


        await listService.DeleteShoppingListAsync(list.Id);
        ShoppingLists.Remove(list);

    }
}
