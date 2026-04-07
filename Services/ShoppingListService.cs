using Firebase.Database;
using Firebase.Database.Query;

using ShareCart.Interfaces;
using ShareCart.Models;

namespace ShareCart.Services;


public class ShoppingListService(IFirebaseProvider provider) : IShoppingListService {
    private readonly FirebaseClient firebase = provider.Client;

    public async Task<string> SaveShoppingListAsync(
        string ownerId,
        string ownerEmail,
        string ownerName,
        ShoppingList cart,
        string nodeName = "ShoppingList") {

        cart.OwnerId = ownerId;
        cart.OwnerEmail = ownerEmail;
        cart.OwnerUsername = ownerName;
        cart.CreatedDate = DateTime.UtcNow;
        cart.UpdatedAt = DateTime.UtcNow;

        var result = await firebase.Child(nodeName).PostAsync(cart);

        cart.Id = result.Key;

        await firebase.Child(nodeName).Child(cart.Id).PutAsync(cart);

        return cart.Id;
    }

    public async Task<string> AddProductAsync(string listId, Product product, string nodeName = "ShoppingList") {
        var result = await firebase
            .Child(nodeName)
            .Child(listId)
            .Child("Products")
            .PostAsync(product);

        return result.Key;
    }

    public async Task<IEnumerable<ShoppingList>> GetShoppingListAsync(
        string ownerId,
        string nodeName = "ShoppingList") {
        var allLists = await firebase
            .Child(nodeName)
            .OnceAsync<ShoppingList>();

        return allLists
            .Where(x =>
                x.Object.OwnerId == ownerId ||
                (x.Object.MemberIds != null && x.Object.MemberIds.Contains(ownerId)))
            .Select(x => {
                var item = x.Object;
                item.Id = x.Key;
                return item;
            });
    }

    public async Task<ShoppingList> GetShoppingListByIdAsync(
        string listId,
        string nodeName = "ShoppingList") {
        var list = await firebase
            .Child(nodeName)
            .Child(listId)
            .OnceSingleAsync<ShoppingList>();

        list?.Id = listId;

        return list!;
    }

    public Task UpdateShoppingListAsync(
        string listId,
        List<string> membersId,
        string nodeName = "ShoppingList") {
        var indexed = new Dictionary<string, string>();

        for(int i = 0 ; i < membersId.Count ; i++)
            indexed[i.ToString()] = membersId[i];

        return firebase
            .Child(nodeName)
            .Child(listId)
            .Child("MemberIds")
            .PutAsync(indexed);
    }

    public Task UpdateShoppingListAsync(
        string listId,
        string ownerUsername,
        string nodeName = "ShoppingList") {
        var updateData = new Dictionary<string, object>
        {
            { "OwnerUsername", ownerUsername }
        };

        return firebase
            .Child(nodeName)
            .Child(listId)
            .PatchAsync(updateData);
    }


    public Task DeleteShoppingListAsync(string listId, string nodeName = "ShoppingList") {
        return firebase
            .Child(nodeName)
            .Child(listId)
            .DeleteAsync();
    }

    public Task DeleteProductAsync(string listId, string productId, string nodeName = "ShoppingList") {
        return firebase
            .Child(nodeName)
            .Child(listId)
            .Child("Products")
            .Child(productId)
            .DeleteAsync();
    }

    public IDisposable SubscribeToList(string listId, Action onProductsChanged) {

        bool isFirstLoad = true;

        return firebase
          .Child("ShoppingList")
          .Child(listId)
          .Child("Products")
          .AsObservable<Product>()
          .Subscribe(_ => {

              if(isFirstLoad) {

                  isFirstLoad = false;
                  return;
              }
              onProductsChanged();
          });

    }
}