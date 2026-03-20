using CommunityToolkit.Mvvm.ComponentModel;

namespace ShareCart.Models;

public partial class UserCredentials : ObservableObject {

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Password { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password);

    partial void OnEmailChanged(string value)
        => OnPropertyChanged(nameof(IsValid));

    partial void OnPasswordChanged(string value)
        => OnPropertyChanged(nameof(IsValid));
}