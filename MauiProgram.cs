using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;

using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

using LocalizationResourceManager.Maui;

using Microsoft.Extensions.Logging;

using ShareCart.Handlers;
using ShareCart.Interfaces;
using ShareCart.Resources.Languages;
using ShareCart.Services;
using ShareCart.ViewModels;
using ShareCart.Views;

using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;
namespace ShareCart;

public static class MauiProgram {

    public static MauiApp CreateMauiApp() {

        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjGyl/VkV+XU9AclRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3hTdURqWH5fd3ddRGNbU091XA==");

        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts => {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("MaterialIcons-Regular.ttf", "Mat");

        }).UseMauiCommunityToolkit().UseLocalizationResourceManager(settings => {
            settings.AddResource(AppResources.ResourceManager);
            settings.RestoreLatestCulture(true);
        });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        BorderlessEntryHandler.Apply();

        builder.Services.AddSingleton<IFirebaseErrorService, FirebaseErrorService>();
        builder.Services.AddSingleton<IUserRepoService, UserRepService>();
        builder.Services.AddSingleton<IShoppingListService, ShoppingListService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IFirebaseProvider, FirebaseProvider>();
        builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        builder.Services.AddSingleton<LoginPageView, LoginPageViewModel>();
        builder.Services.AddSingleton<HomePageView, HomePageViewModel>();
        builder.Services.AddTransient<UserProfilePageView, UserProfilePageViewModel>();
        builder.Services.AddTransient<AddItemToCartPageView, AddItemToCartPageViewModel>();
        builder.Services.AddTransient<ShoppingDetailsPageView, ShoppingDetailsPageViewModel>();


        builder.Services.AddSingleton<IFirebaseAuthClient>(provider => {
            var config = new FirebaseAuthConfig {

                ApiKey = "AIzaSyDX_wYkDhfNgaDhS92xqgXPABiJH8Z_vdA",
                AuthDomain = "sharecart-5c350.firebaseapp.com",
                Providers = [
                    new EmailProvider()
                ],
                UserRepository = new FileUserRepository("ShareCart")
            };

            return new FirebaseAuthClient(config);
        });

        builder.ConfigureSyncfusionCore();
        return builder.Build();
    }
}