namespace PetAdoption.Mobile.Pages;

public partial class ListChatPage : ContentPage
{
    private readonly ListChatPageViewModel _viewModel;

    public ListChatPage(ListChatPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.DisposeAsync();
        await _viewModel.InitializeAsync();
    }

}