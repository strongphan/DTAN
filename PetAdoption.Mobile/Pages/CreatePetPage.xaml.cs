namespace PetAdoption.Mobile.Pages;

public partial class CreatePetPage : ContentPage
{
    private readonly CreatePetViewModel _viewModel;

    public CreatePetPage(CreatePetViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}