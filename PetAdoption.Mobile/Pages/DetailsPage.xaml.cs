namespace PetAdoption.Mobile.Pages;

public partial class DetailsPage : ContentPage
{
    private readonly DetailViewModel _viewModel;

    public DetailsPage(DetailViewModel viewModel)
    {
        InitializeComponent();
        this._viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await _viewModel.DisposeAsync();
    }
}