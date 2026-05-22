using Firebase.Auth;
using Firebase.Auth.Providers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LandersSuperstore.Pages;

public partial class CreateAccountPage : ContentPage
{

    private const string FirebaseApiKey = "AIzaSyCIG23MKpX5mz0zBuxe_P4KqoO7zFRWFQw";
    private const string FirebaseProjectId = "subrio-sfact1";

    private readonly FirebaseAuthClient _authClient;

    public CreateAccountPage()
    {
        InitializeComponent();

        
        var config = new FirebaseAuthConfig
        {
            ApiKey = "AIzaSyCIG23MKpX5mz0zBuxe_P4KqoO7zFRWFQw",
            AuthDomain = "subrio-sfact1.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        };

        _authClient = new FirebaseAuthClient(config);
    }

    private async void OnCreateClicked(object? sender, EventArgs e)
    {

        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;

       
        if (string.IsNullOrWhiteSpace(FirstnameEntry.Text))
        {
            ShowError("Full Name is required.");
            return;
        }
        if (string.IsNullOrWhiteSpace(LastnameEntry.Text))
        {
            ShowError("Full Name is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            ShowError("Email address is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ShowError("Password is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
        {
            ShowError("Please confirm your password.");
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ShowError("Passwords do not match.");
            return;
        }

        if (PasswordEntry.Text.Length < 6)
        {
            ShowError("Password must be at least 6 characters.");
            return;
        }

        if (RolePicker.SelectedIndex == -1)
        {
            ShowError("Please select a role.");
            return;
        }

     
        try
        {
            // Creates user in Firebase Authentication
            var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(
                EmailEntry.Text.Trim(),
                PasswordEntry.Text
            );

            string uid = userCredential.User.Uid;
            string token = await userCredential.User.GetIdTokenAsync();

           
            await SaveUserToFirestore(uid, token);

          
            await DisplayAlert("Success", "Account created successfully!", "OK");
            await Navigation.PushAsync(new HomePage());
        }
        catch (FirebaseAuthException ex)
        {
            
            ShowError(GetFriendlyFirebaseError(ex.Reason));
        }
        catch (Exception ex)
        {
            ShowError($"Unexpected error: {ex.Message}");
        }
    }

    
    private async Task SaveUserToFirestore(string uid, string idToken)
    {
        using var httpClient = new HttpClient();


        string url = $"https://firestore.googleapis.com/v1/projects/{FirebaseProjectId}" +
                     $"/databases/(default)/documents/users/{uid}?access_token={idToken}";


        var firestoreDocument = new
        {
            fields = new
            {
                Firstname = new { stringValue = FirstnameEntry.Text.Trim() },
                Lastname = new { stringValue = LastnameEntry.Text.Trim() },
                email = new { stringValue = EmailEntry.Text.Trim() },
                role = new { stringValue = RolePicker.SelectedItem.ToString() },
                createdAt = new { timestampValue = DateTime.UtcNow.ToString("o") },
                uid = new { stringValue = uid }
            }
        };

        var json = JsonSerializer.Serialize(firestoreDocument);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        var response = await httpClient.PatchAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Firestore save failed: {error}");
        }
    }

  
    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }

  
    private string GetFriendlyFirebaseError(AuthErrorReason reason)
    {
        return reason switch
        {
            AuthErrorReason.EmailExists => "This email is already registered.",
            AuthErrorReason.InvalidEmailAddress => "The email address is not valid.",
            AuthErrorReason.WeakPassword => "Password is too weak. Use at least 6 characters.",
            AuthErrorReason.OperationNotAllowed => "Email/password sign-up is not enabled.",
            AuthErrorReason.TooManyAttemptsTryLater => "Too many attempts. Please try again later.",
            _ => "Account creation failed. Please try again."
        };
    }
}