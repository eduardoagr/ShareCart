using CommunityToolkit.Maui;

using Firebase.Auth;
using Firebase.Auth.Providers;

using LocalizationResourceManager.Maui;

using Microsoft.Extensions.Logging;

using ShareCart.Handlers;
using ShareCart.Interfaea;
using ShareCart.Resources.Languages;
using ShareCart.Services;
using ShareCart.ViewModels;
using ShareCart.Views;

using Syncfusion.Maui.Core.Hosting;

namespace ShareCart {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
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
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();

            builder.Services.AddSingleton<LoginPageView, LoginPageViewModel>();
            builder.Services.AddTransient<RegistrationPageView>();


            builder.Services.AddSingleton(provider => {
                var config = new FirebaseAuthConfig {
                    ApiKey = "AIzaSyDX_wYkDhfNgaDhS92xqgXPABiJH8Z_vdA",
                    AuthDomain = "sharecart-5c350.firebaseapp.com",
                    Providers = [new EmailProvider()]
                };

                return new FirebaseAuthClient(config);
            });

            builder.ConfigureSyncfusionCore();
            return builder.Build();
        }
    }
}