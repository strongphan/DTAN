namespace PetAdoption.Mobile.Controls;

public partial class ProfileOptionRow : ContentView
{
    public ProfileOptionRow()
    {
        InitializeComponent();
    }
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(PetFeatControl));
    public string Option
    {
        get => (string)GetValue(OptionProperty);
        set => SetValue(OptionProperty, value);
    }
    public static readonly BindableProperty OptionProperty =
    BindableProperty.Create(nameof(Option), typeof(string), typeof(PetFeatControl));

    public event EventHandler<string> Tapped;

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Tapped?.Invoke(this, Option);
    }
}