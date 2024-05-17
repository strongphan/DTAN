namespace PetAdoption.Mobile.Pages;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoriteViewModel _viewModel;

    public FavoritesPage(FavoriteViewModel viewModel)
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