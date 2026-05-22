using Firebase.Auth;
namespace LandersSuperstore.Pages;


    public partial class LoginPage : ContentPage
    {
    private readonly FirebaseAuthClient _authClient;
    private Picker RolePicker;
        public LoginPage(FirebaseAuthClient authClient)
        {
            InitializeComponent();
            _authClient = authClient;
        RolePicker = new Picker
        {
            Title = "Select Role",
            ItemsSource = new List<string> { "User", "Seller", "Delivery Rider" }
        };

    }
    private async void OnLogInClicked(object? sender, EventArgs e)
        {
        try
        {
            
            if (RolePicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please select a role before logging in.", "OK");
                return;
            }

            var user = await _authClient.SignInWithEmailAndPasswordAsync(EmailEntry.Text, PasswordEntry.Text);

            
            string selectedRole = RolePicker.SelectedItem.ToString();

            await DisplayAlert("Success", "Log In Successful", "OK");

            
            switch (selectedRole)
            {
                case "User":
                    await Navigation.PushAsync(new HomePage());
                    break;

                case "Seller":
                    await Navigation.PushAsync(new SellerHomePage());
                    break;

                case "Delivery Rider":
                    await Navigation.PushAsync(new DeliveryRiderPage());
                    break;

                default:
                    await DisplayAlert("Error", "Invalid role selected.", "OK");
                    break;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


        
        private async void OnCreateClicked(object? sender, EventArgs e)
        {

            await Navigation.PushAsync(new CreateAccountPage());
        }

        private async void OnForgotClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
    }