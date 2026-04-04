using LocalizationResourceManager.Maui;

using ShareCart.Interfaces;

namespace ShareCart.Services;

public class FirebaseErrorService(ILocalizationResourceManager localization) : IFirebaseErrorService {

    public string GetMessage(Exception ex) {

        var message = ex.Message ?? string.Empty;

        if(message.Contains("INVALID_LOGIN_CREDENTIALS"))
            return localization["UI_WrongPassword"];

        if(message.Contains("EMAIL_NOT_FOUND"))
            return localization["UI_EmailNotRegistered"];

        if(message.Contains("INVALID_EMAIL"))
            return localization["UI_InvalidEmail"];

        if(message.Contains("USER_DISABLED"))
            return localization["UI_UserDisabled"];

        if(message.Contains("EMAIL_EXISTS"))
            return localization["UI_EmailExists"];

        if(ex is HttpRequestException)
            return localization["UI_NetworkError"];

        if(ex is TaskCanceledException)
            return localization["UI_Timeout"];

        return localization["UI_UnexpectedError"];
    }
}