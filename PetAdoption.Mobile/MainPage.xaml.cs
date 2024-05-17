using PetAdoption.Mobile.Pages;

namespace PetAdoption.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Check if onboarding screen show
            if (Preferences.Default.ContainsKey(UIConstants.OnboardingShown))
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            else
                await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}");

        }
    }
}