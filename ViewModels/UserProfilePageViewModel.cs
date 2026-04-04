using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ShareCart.Interfaces;
using ShareCart.Models;

namespace ShareCart.ViewModels {

    public partial class UserProfilePageViewModel(IUserRepoService userRepoService, IAuthService authService) : ObservableObject {

        [ObservableProperty]
        public partial FirebaseUser? FirebaseUser { get; set; }

        [RelayCommand]
        async Task LoadUserDataAsync() {
            FirebaseUser = await userRepoService.GetFirebaseUser(authService.GetAuthUserID());
        }

        [RelayCommand]
        async Task SaveUserDataAsync() {

            if(FirebaseUser is null) {
                return;
            }

            await userRepoService.UpdateFirebaseUser(authService.GetAuthUserID(),
                $"{FirebaseUser.Name}".TrimEnd(), null);

            await Shell.Current.GoToAsync("..");
        }
    }
}
