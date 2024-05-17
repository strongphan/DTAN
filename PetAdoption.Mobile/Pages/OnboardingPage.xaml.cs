namespace PetAdoption.Mobile.Pages;

public partial class OnboardingPage : ContentPage
{
    public OnboardingPage()
    {
        InitializeComponent();

        Preferences.Default.Set(UIConstants.OnboardingShown, string.Empty);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var pram = new Dictionary<string, object>
        {
            [nameof(LoginRegisterViewModel.IsFirstTime)] = true,
        };
        await Shell.Current.GoToAsync($"//{nameof(LoginRegisterPage)}", pram);
    }
}