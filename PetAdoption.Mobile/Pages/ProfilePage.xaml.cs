namespace PetAdoption.Mobile.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void ProfileOptionRow_Tapped(object sender, string optionText)
    {
        switch (optionText)
        {
            case "myAdoptions":
                await _viewModel.GoToAsync(nameof(AdoptionsPage));
                break;
            case "changePassword":
                await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
                break;
            case "myPets":
                await _viewModel.GoToAsync(nameof(OwnersPage));
                break;
            case "myProfile":
                await _viewModel.GoToAsync(nameof(EditProfilePage));
                break;
            default: break;
        }
    }


}