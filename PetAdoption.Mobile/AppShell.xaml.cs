namespace PetAdoption.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AdoptionSuccessPage), typeof(AdoptionSuccessPage));
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) => await Launcher.OpenAsync("https://www.youtube.com/@abhayprince");
    }
}