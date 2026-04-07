using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using LocalizationResourceManager.Maui;

using ShareCart.Interfaces;
using ShareCart.Models;

using System.Collections.ObjectModel;

namespace ShareCart.ViewModels;

public partial class AddItemToCartPageViewModel : ObservableObject {

    private readonly IShoppingListService shoppingListService;
    private readonly IAuthService authService;
    private readonly IMessenger messenger;
    private readonly IUserRepoService userRepoService;
    private readonly ILocalizationResourceManager localizationResource;

    public Action? ScrollToEndRequested;

    public Action? FocusLastEntryRequested;

    public ShoppingList CurrentList { get; set; } = new();

    public ObservableCollection<Product> Products { get; set; } = [new()];

    public ObservableCollection<FirebaseUser> FirebaseUsers { get; set; } = [new()];

    public ObservableCollection<FirebaseUser> Ids { get; set; } = [];

    [ObservableProperty]
    public partial FirebaseUser? CurrentUser { get; set; }

    [ObservableProperty]
    public partial bool IsSharePopupOpen { get; set; }

    public bool IsShared { get; private set; }

    [ObservableProperty]
    public partial bool HasUsersToShare { get; set; }

    public AddItemToCartPageViewModel(IShoppingListService shoppingListService, IAuthService authService,
        IMessenger messenger, IUserRepoService userRepoService, ILocalizationResourceManager localizationResource) {

        this.shoppingListService = shoppingListService;
        this.authService = authService;
        this.messenger = messenger;
        this.userRepoService = userRepoService;
        this.localizationResource = localizationResource;

        Task.Run(GetAlUsers);

    }

    [RelayCommand]
    void AddOrDeleteNewRows(string action) {

        if(action == "1") {

            foreach(var item in Products) {

                item.ShouldFocus = false;
            }

            Products.Add(new Product { ShouldFocus = true });
            ScrollToEndRequested?.Invoke();
            return;
        }

        if(action == "0" && Products.Count > 1) {
            Products.RemoveAt(Products.Count - 1);
        }
    }

    public async Task CheckStatus(Product product, bool value) {

        if(product is null) {
            return;
        }

        if(value) {
            Products.Remove(product);
            if(Products.Count < 1) {
                await SaveListAsync();
            }
        }
    }

    async Task GetAlUsers() {

        FirebaseUsers.Clear();

        var users = await userRepoService.GetFirebaseUsers();

        foreach(var user in users) {
            if(user.Id != authService.GetAuthUserID()) {
                FirebaseUsers.Add(user);
            }
        }
    }

    [RelayCommand]
    async Task SaveDatabase() {

        await SaveListAsync();

        HasUsersToShare = !string.IsNullOrEmpty(CurrentList.Id) && FirebaseUsers.Count > 0;
    }

    private async Task SaveListAsync() {

        var tempProducts = Products.Where(p => !string.IsNullOrWhiteSpace(p.Name))
            .ToList();

        CurrentList.Products = [];

        var user = await GetUser();

        var id = await shoppingListService.SaveShoppingListAsync(
            authService.GetAuthUserID(),
            authService.GetAuthUserEmail(),
            user.Username,
            CurrentList
        );

        if(string.IsNullOrWhiteSpace(CurrentList.Id)) {
            CurrentList.Id = id;
        }

        foreach(var product in tempProducts) {

            product.AddedById = authService.GetAuthUserID();

            var productId = await shoppingListService.AddProductAsync(CurrentList.Id, product);

        }

        messenger.Send(CurrentList);
    }

    private async Task<FirebaseUser> GetUser() {

        return await userRepoService.GetFirebaseUser(authService.GetAuthUserID());
    }

    [RelayCommand]
    async Task ControlPopUpState() {

        IsSharePopupOpen = true;
    }


    [RelayCommand]
    async Task SharePopUpClosed() {
        IsSharePopupOpen = false;

        var FirebaseUsersId = Ids.Select(x => x.Id).ToList();
        await shoppingListService.UpdateShoppingListAsync(CurrentList.Id, FirebaseUsersId);
    }

}
