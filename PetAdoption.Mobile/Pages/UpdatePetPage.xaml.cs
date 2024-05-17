namespace PetAdoption.Mobile.Pages;

public partial class UpdatePetPage : ContentPage
{
    private readonly UpdatePetViewModel _viewModel;

    public UpdatePetPage(UpdatePetViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}