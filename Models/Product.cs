using Newtonsoft.Json;

namespace ShareCart.Models;

public class Product {

    public string Id { get; set; }

    [JsonIgnore]
    public bool IsChecked { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<FirebaseUser> Addedby { get; set; } = [];

    [JsonIgnore]
    public bool ShouldFocus { get; set; }
}