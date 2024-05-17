namespace PetAdoption.Mobile.Pages;

public partial class AllPetsPage : ContentPage
{
    private readonly AllPetsViewModel _viewModel;

    public AllPetsPage(AllPetsViewModel viewModel)
    {
        InitializeComponent();
        this._viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}