using ShareCart.Models;

namespace ShareCart.Interfaces;

public interface IShoppingListService {

    Task<string> AddProductAsync(string listId, Product product, string nodeName = "ShoppingList");

    Task DeleteProductAsync(string listId, string productId, string nodeName = "ShoppingList");

    Task<string> SaveShoppingListAsync(string ownerId, string ownerEmail, string OwnerName, ShoppingList cart, string nodeName = "ShoppingList");

    Task<IEnumerable<ShoppingList>> GetShoppingListAsync(string ownerId, string nodeName = "ShoppingList");

    Task DeleteShoppingListAsync(string listId, string nodeName = "ShoppingList");

    Task UpdateShoppingListAsync(string listId, List<string> MembersId, string nodeName = "ShoppingList");

    IDisposable SubscribeToShoppingList(string ownerId, Action<ShoppingList?> onChanged, Action<Exception>? onError = null, string nodeName = "ShoppingList");
}