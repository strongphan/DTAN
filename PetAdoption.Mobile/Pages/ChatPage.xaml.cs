namespace PetAdoption.Mobile.Pages;

public partial class ChatPage : ContentPage
{
    private readonly ChatPageViewModel _viewModel;

    public ChatPage(ChatPageViewModel viewModel)
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