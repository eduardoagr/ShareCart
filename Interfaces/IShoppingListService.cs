using ShareCart.Models;

namespace ShareCart.Interfaces;

public interface IShoppingListService {

    Task<string> AddProductAsync(string listId, Product product, string nodeName = "ShoppingList");

    Task DeleteProductAsync(string listId, string productId, string nodeName = "ShoppingList");

    Task<string> SaveShoppingListAsync(string ownerId, string ownerEmail,
        string OwnerUsername, ShoppingList cart, string nodeName = "ShoppingList");

    Task<IEnumerable<ShoppingList>> GetShoppingListAsync(string ownerId, string nodeName = "ShoppingList");

    Task<ShoppingList> GetShoppingListByIdAsync(string listId, string nodeName = "ShoppingList");

    Task DeleteShoppingListAsync(string listId, string nodeName = "ShoppingList");

    Task UpdateShoppingListAsync(string listId, List<string> MembersId, string nodeName = "ShoppingList");

    Task UpdateShoppingListAsync(string listId, string OwnerUsername, string nodeName = "ShoppingList");

    public IDisposable SubscribeToList(string listId, Action onProductsChanged);


}