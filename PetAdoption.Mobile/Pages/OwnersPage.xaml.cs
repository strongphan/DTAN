namespace PetAdoption.Mobile.Pages;

public partial class OwnersPage : ContentPage
{
    private readonly OwnerViewModel _viewModel;

    public OwnersPage(OwnerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}