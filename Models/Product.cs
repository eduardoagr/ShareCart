using CommunityToolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;

namespace ShareCart.Models;

public partial class Product : ObservableObject {

    [JsonIgnore]
    public string Id { get; internal set; }

    [JsonIgnore]
    public bool IsChecked { get; set; }

    public string Name { get; set; } = string.Empty;

    public string AddedById { get; set; }

    [JsonIgnore]
    public FirebaseUser AddedBy { get; set; }

    // Local UI-only state
    [JsonIgnore]
    public bool ShouldFocus { get; set; }

}