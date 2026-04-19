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
                Refresh();
                if(Products.Count <= 0) {

                    await shoppingListService.DeleteShoppingListAsync(id);

                    Cleanup();

                    messenger.Send("Let's go home");

                    await Shell.Current.GoToAsync("..", true);


                }
            });

        });
    }

    private async Task Refresh() {
        await LoadMembersAsync();
        await InitProductList();
    }

    private async Task InitProductList() {
        if(Products == null)
            return;

        await HydrateProductsAsync();

        // 🔥 Capture existing empty row BEFORE clearing
        var pendingEmpty = Products.FirstOrDefault(p => string.IsNullOrWhiteSpace(p.Name));

        Products.Clear();

        if(ShoppingList?.Products != null) {
            foreach(var item in ShoppingList.Products) {
                var product = item.Value;
                product.Id = item.Key;
                Products.Add(product);
            }
        }

        // 🔥 Re-add the empty row AFTER refresh
        if(pendingEmpty != null) {
            Products.Add(pendingEmpty);
        }
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

        foreach(var p in Products)
            p.ShouldFocus = false;

        var newItem = new Product {
            ShouldFocus = false,
            AddedById = currentUser.Id,
            AddedBy = currentUser
        };

        Products.Add(newItem);

        ScrollToEndRequested?.Invoke();

        MainThread.BeginInvokeOnMainThread(async () => {
            await Task.Delay(50);
            newItem.ShouldFocus = true;
        });
    }

    [RelayCommand]
    async Task UploadAndAddNext(Product product) {
        if(product == null || ShoppingList == null)
            return;

        // 1. Save current item
        await shoppingListService.AddProductAsync(ShoppingList.Id, product);

        var currentUser = await userRepoService.GetFirebaseUser(authService.GetAuthUserID());

        // 2. Clear ALL focus flags (prevents recycled cell bugs)
        foreach(var p in Products)
            p.ShouldFocus = false;

        // 3. Create NEW row (this is the key difference)
        var newItem = new Product {
            Name = string.Empty,
            ShouldFocus = false,
            AddedById = currentUser.Id,
            AddedBy = currentUser
        };

        Products.Add(newItem);

        // 4. Scroll to bottom
        ScrollToEndRequested?.Invoke();

        // 5. Focus the new row AFTER UI updates
        MainThread.BeginInvokeOnMainThread(async () => {
            await Task.Delay(50);
            newItem.ShouldFocus = true;
        });
    }
}