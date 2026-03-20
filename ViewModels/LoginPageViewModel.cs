using CommunityToolkit.Mvvm.Input;

using LocalizationResourceManager.Maui;

using ShareCart.Interfaea;
using ShareCart.Models;

namespace ShareCart.ViewModels;

public partial class LoginPageViewModel(IAuthService auth, IFirebaseErrorService errors,
    ILocalizationResourceManager resourceManager, IUserRepository userRepository) : AuthBaseViewModel {

    protected override void OnCredentialsChanged() {
        LoginCommand.NotifyCanExecuteChanged();
        CloseRegisterCommand.NotifyCanExecuteChanged();
    }

    private bool IsReady() => Credentials.IsValid;

    [RelayCommand(CanExecute = nameof(IsReady))]
    public async Task LoginAsync() {
        try {

            isBusy = true;

            var authUser = await auth.LoginAsync(Credentials.Email, Credentials.Password);

            if(authUser != null) {

                User fireUser = new() {
                    Id = authUser.User.Uid,
                    Email = Credentials.Email,
                    CreatedAt = DateTime.UtcNow.ToString("g"),
                    LastLogin = DateTime.UtcNow.ToString("g"),
                };

                await userRepository.SaveUserEmailAsync("users", fireUser);
            }

        } catch(Exception ex) {

            Message = errors.GetMessage(ex);

            isFailed = true;

        } finally {

            isBusy = false;
        }
    }

    [RelayCommand]
    public void OpenRegister() {

        isRegisterPopupOpen = true;
    }

    [RelayCommand(CanExecute = nameof(IsReady))]
    public async Task CloseRegister() {

        isRegisterPopupOpen = false;

        try {

            isBusy = true;

            await auth.RegisterAsync(Credentials.Email, Credentials.Password);

        } catch(Exception ex) {

            Message = errors.GetMessage(ex);

            isFailed = true;

        } finally {

            isBusy = false;
            await Shell.Current.DisplayAlertAsync(resourceManager["UI_Success"],
                resourceManager["UI_RegistrationSuccess"], resourceManager["UI_Ok"]);
        }
    }
}