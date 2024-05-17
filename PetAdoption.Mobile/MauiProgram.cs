using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PetAdoption.Mobile.Hub;
using PetAdoption.Shared;
using Refit;

namespace PetAdoption.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Ubuntu-Regular.ttf", "UbuntuRegular");
                    fonts.AddFont("Ubuntu-Bold.ttf", "UbuntuBold");
                })
                .UseMauiCommunityToolkit();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            RegisterAppDependencies(builder.Services);
            ConfigureRefit(builder.Services);
            return builder.Build();
        }
        static void RegisterAppDependencies(IServiceCollection services)
        {
            services.AddSingleton<CommonService>();

            services.AddTransient<AuthService>();
            services.AddSingleton<ChatHub>();

            services.AddTransient<LoginRegisterViewModel>()
                .AddTransient<LoginRegisterPage>();

            services.AddTransient<HomeViewModel>()
                .AddTransient<HomePage>();

            services.AddTransient<AllPetsViewModel>()
                .AddTransient<AllPetsPage>();

            services.AddTransient<ProfileViewModel>()
                .AddTransient<ProfilePage>();

            services.AddTransient<FavoriteViewModel>()
                .AddTransient<FavoritesPage>();

            services.AddSingleton<ListChatPageViewModel>()
                .AddSingleton<ListChatPage>();

            services.AddTransientWithShellRoute<ChatPage, ChatPageViewModel>(nameof(ChatPage));

            services.AddTransientWithShellRoute<DetailsPage, DetailViewModel>(nameof(DetailsPage));

            services.AddTransientWithShellRoute<AdoptionsPage, MyAdoptionsViewModel>(nameof(AdoptionsPage));

            services.AddTransientWithShellRoute<OwnersPage, OwnerViewModel>(nameof(OwnersPage));
            services.AddTransientWithShellRoute<CreatePetPage, CreatePetViewModel>(nameof(CreatePetPage));
            services.AddTransientWithShellRoute<UpdatePetPage, UpdatePetViewModel>(nameof(UpdatePetPage));
        }
        static void ConfigureRefit(IServiceCollection services)
        {
            services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IPetsApi>()
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IUsersApi>(sp =>
            {
                var commonService = sp.GetRequiredService<CommonService>();
                return new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = (_, __)
                    => Task.FromResult(commonService.Token ?? string.Empty),
                };
            })
                .ConfigureHttpClient(SetHttpClient);

            static void SetHttpClient(HttpClient httpClient) =>
                httpClient.BaseAddress = new Uri(AppConstants.BaseURL);
        }
    }


}