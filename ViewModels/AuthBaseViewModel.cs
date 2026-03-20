using CommunityToolkit.Mvvm.ComponentModel;

using ShareCart.Models;

namespace ShareCart.ViewModels;

public partial class AuthBaseViewModel : ObservableObject {

    public AuthBaseViewModel() {
        Credentials.PropertyChanged += (_, e) => {
            if(e.PropertyName == nameof(UserCredentials.IsValid))
                OnCredentialsChanged();
        };
    }

    [ObservableProperty]
    public partial UserCredentials Credentials { get; set; } = new();

    [ObservableProperty]
    public partial bool isBusy { get; set; }

    [ObservableProperty]
    public partial bool isFailed { get; set; }

    [ObservableProperty]
    public partial bool isRegisterPopupOpen { get; set; }

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;

    protected virtual void OnCredentialsChanged() {
        // Los hijos sobrescriben esto
    }
}