using Newtonsoft.Json;

namespace ShareCart.Models;

public class ShoppingList {

    public string Id { get; set; } = string.Empty;

    public string OwnerId { get; set; } = string.Empty;

    public string OwnerEmail { get; set; } = string.Empty;

    public string OwnerUsername { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Dictionary<string, Product> Products { get; set; } = [];

    public List<string> MemberIds { get; set; } = [];

    [JsonIgnore]
    public bool IsMine { get; set; }

}
