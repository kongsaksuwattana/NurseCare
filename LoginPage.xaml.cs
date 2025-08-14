using NurseCare.DataAccess;
namespace NurseCare;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        // Auto-fill if remembered
        if (Preferences.ContainsKey("username") && Preferences.ContainsKey("password"))
        {
            UsernameEntry.Text = Preferences.Get("username", "");
            PasswordEntry.Text = Preferences.Get("password", "");
            RememberCheckbox.IsChecked = true;
        }


    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        string? personId = UserStore.GetPersonId(username,password);
        //if (UserStore.Validate(username, password))
        if(!string.IsNullOrEmpty(personId))
        {
            if (RememberCheckbox.IsChecked)
            {
                Preferences.Set("username", username);
                Preferences.Set("password", password);
            }
            else
            {
                Preferences.Remove("username");
                Preferences.Remove("password");
            }

            NurseCareDataQuery dataQuery = new NurseCareDataQuery();
            var nurse = dataQuery.GetNursebyId(personId);
            if (nurse == null)
            {
                await DisplayAlert("Error", "Nurse not found", "OK");
                return;
            }
            Preferences.Set("personId", personId); 
            Preferences.Set("firstName", nurse.FirstName);
            Preferences.Set("lastName", nurse.LastName);
            Preferences.Set("team", nurse.TeamName.ToString());
            await DisplayAlert("Success", $"Logged in! by {username}", "OK");
            // Navigate to main app page
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Error", "Invalid credentials", "OK");
        }

    }

    private void RegisterButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new UserRegister());
    }
}