using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ShareCart.Interfaces;
using ShareCart.Models;

using System.Collections.ObjectModel;

namespace ShareCart.ViewModels;

public partial class ShoppingDetailsPageViewModel(
    IUserRepoService userRepoService,
    IShoppingListService shoppingListService,
    IAuthService authService,
    IMessenger messenger) : ObservableObject, IQueryAttributable {

    private IDisposable? _subscription;

    public Action? ScrollToEndRequested;


    [ObservableProperty]
    public partial ShoppingList? ShoppingList { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Product> Products { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<FirebaseUser> ShoppingCartMembers { get; set; } = [];

    public async void ApplyQueryAttributes(IDictionary<string, object> query) {

        if(!query.TryGetValue("listId", out var rawId))
            return;

        var id = rawId as string;
        if(string.IsNullOrEmpty(id))
            return;

        // Load list
        ShoppingList = await shoppingListService.GetShoppingListByIdAsync(id);

        if(ShoppingList == null)
            return;

        await LoadMembersAsync();
        await InitProductList();

        _subscription = shoppingListService.SubscribeToList(id, async () => {
            MainThread.BeginInvokeOnMainThread(async () => {
                ShoppingList = await shoppingListService.GetShoppingListByIdAsync(id);
                await LoadMembersAsync();
                await InitProductList();
                if(Products.Count <= 0) {

                    await shoppingListService.DeleteShoppingListAsync(id);

                    Cleanup();

                    messenger.Send("Let's go home");

                    await Shell.Current.GoToAsync("..", true);


                }
            });

        });
    }

    private async Task InitProductList() {

        if(Products == null)
            return;

        await HydrateProductsAsync();

        Products.Clear();

        if(ShoppingList?.Products == null)
            return;

        foreach(var item in ShoppingList.Products) {

            var product = item.Value;
            product.Id = item.Key;
            Products.Add(product);
        }

        Products.LastOrDefault()?.ShouldFocus = true;
    }

    private async Task LoadMembersAsync() {

        if(ShoppingList == null)
            return;

        ShoppingCartMembers.Clear();

        var owner = await userRepoService.GetFirebaseUser(ShoppingList.OwnerId);

        if(owner != null)

            ShoppingCartMembers.Add(owner);

        if(ShoppingList.MemberIds != null) {
            foreach(var id in ShoppingList.MemberIds) {
                var member = await userRepoService.GetFirebaseUser(id);
                if(member != null)
                    ShoppingCartMembers.Add(member);
            }
        }
    }

    public async Task HydrateProductsAsync() {

        if(ShoppingList?.Products == null)
            return;

        foreach(var item in ShoppingList.Products.Values) {

            if(!string.IsNullOrEmpty(item.AddedById)) {

                item.AddedBy = await userRepoService.GetFirebaseUser(item.AddedById);
            }

        }
    }

    public void Cleanup() {
        _subscription?.Dispose();
        _subscription = null;
    }

    [RelayCommand]
    async Task CheckStatus(Product product) {

        if(product == null) return;

        if(ShoppingList == null) return;

        await shoppingListService.DeleteProductAsync(ShoppingList.Id, product.Id);

    }

    [RelayCommand]
    async Task AddProduct() {

        var currentUser = await userRepoService.GetFirebaseUser(authService.GetAuthUserID());

        Products.Add(new Product {
            ShouldFocus = true,
            AddedById = currentUser.Id,
            AddedBy = currentUser
        });

        ScrollToEndRequested?.Invoke();

    }

    [RelayCommand]
    async Task Upload(Product product) {

        if(product == null) return;

        await shoppingListService.AddProductAsync(ShoppingList!.Id, product);

        product.Name = string.Empty;

    }
}