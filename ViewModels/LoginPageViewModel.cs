using CommunityToolkit.Mvvm.Input;

using ShareCart.Interfaces;
using ShareCart.Models;
using ShareCart.Views;

namespace ShareCart.ViewModels;

public partial class LoginPageViewModel(IAuthService auth, IFirebaseErrorService errors,
    IUserRepoService userRepository) : AuthBaseViewModel {

    protected override void OnCredentialsChanged() {
        LoginCommand.NotifyCanExecuteChanged();
        CloseRegisterCommand.NotifyCanExecuteChanged();
    }

    private bool IsReady() => Credentials.IsValid;

    [RelayCommand(CanExecute = nameof(IsReady))]
    async Task LoginAsync() {
        try {

            isBusy = true;

            var authUser = await auth.LoginAsync(Credentials.Email, Credentials.Password);

            await Shell.Current.GoToAsync($"//{nameof(HomePageView)}", true);

        } catch(Exception ex) {

            Message = errors.GetMessage(ex);

            isFailed = true;

        } finally {

            isBusy = false;

        }
    }

    [RelayCommand]
    void OpenRegister() {

        isRegisterPopupOpen = true;
    }

    [RelayCommand(CanExecute = nameof(IsReady))]
    async Task CloseRegister() {

        isRegisterPopupOpen = false;

        try {

            isBusy = true;

            var authUser = await auth.RegisterAsync(
                Credentials.Email, Credentials.Password);


            if(authUser is not null && !string.IsNullOrEmpty(authUser.User.Uid)) {

                FirebaseUser fireUser = new() {
                    Id = authUser.User.Uid,
                    Email = Credentials.Email,
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                };

                var isSaved = await userRepository.SaveUserAsync(fireUser);

                if(isSaved) {
                    await Shell.Current.GoToAsync($"//{nameof(HomePageView)}", true);
                }
            }

        } catch(Exception ex) {

            Message = errors.GetMessage(ex);

            isFailed = true;

        } finally {

            isBusy = false;
        }
    }
}