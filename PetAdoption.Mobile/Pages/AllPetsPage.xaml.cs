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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _viewModel.SearchCommand.Execute(null);
        }
    }
}