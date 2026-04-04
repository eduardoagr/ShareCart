using Firebase.Database;
using Firebase.Database.Query;

using ShareCart.Interfaces;
using ShareCart.Models;

namespace ShareCart.Services;


public class ShoppingListService(IFirebaseProvider provider) : IShoppingListService {

    private readonly FirebaseClient firebase = provider.Client;

    public async Task<IEnumerable<ShoppingList>> GetShoppingListAsync(string ownerId, string nodeName = "ShoppingList") {

        var allLists = await firebase
            .Child(nodeName)
            .OnceAsync<ShoppingList>();


        return allLists
            .Where(x =>
                x.Object.OwnerId == ownerId ||
                (x.Object.MemberIds != null && x.Object.MemberIds.Contains(ownerId))
            )
            .Select(x => {
                var item = x.Object;
                item.Id = x.Key;
                return item;
            });

    }

    public async Task<string> SaveShoppingListAsync(string OwnerId, string OwnerEmail, string OwnerName, ShoppingList cart, string nodeName = "ShoppingList") {

        cart.OwnerId = OwnerId;
        cart.OwnerEmail = OwnerEmail;
        cart.UpdatedAt = DateTime.UtcNow;
        cart.CreatedDate = DateTime.UtcNow;
        cart.OwnerName = OwnerName;

        var result = await firebase.Child(nodeName).PostAsync(cart);

        cart.Id = result.Key;

        await firebase.Child(nodeName).Child(cart.Id).PutAsync(cart);

        return cart.Id;
    }

    public IDisposable SubscribeToShoppingList(string ownerId, Action<ShoppingList?> onChanged, Action<Exception>? onError = null,
        string nodeName = "ShoppingList") {

        return firebase.Child(nodeName).Child(ownerId)
        .AsObservable<ShoppingList>()
        .Subscribe(
            x => {
                if(x.Object != null)
                    onChanged(x.Object);
            },
            ex => onError?.Invoke(ex)
        );

    }

    public Task UpdateShoppingListAsync(string listId, List<string> MembersId, string nodeName = "ShoppingList") {

        var indexed = new Dictionary<string, string>();

        for(int i = 0 ; i < MembersId.Count ; i++)
            indexed[i.ToString()] = MembersId[i];

        return firebase
            .Child(nodeName)
            .Child(listId)
            .Child("MemberIds")
            .PutAsync(indexed);
    }

    public Task DeleteShoppingListAsync(string ListId, string nodeName = "ShoppingList") {

        return firebase
            .Child(nodeName)
            .Child(ListId)
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

    public async Task<string> AddProductAsync(string listId, Product product, string nodeName = "ShoppingList") {

        var result = await firebase
            .Child(nodeName)
            .Child(listId)
            .Child("Products")
            .PostAsync(product);

        return result.Key;

    }
}
