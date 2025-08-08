using NurseCare.DataAccess;
using NurseCare.Model;
using System.Windows.Input;
namespace NurseCare;

public partial class UserRegister : ContentPage
{
	private string[] NurseTeam = new string[]
	{
		"Team A","Team B","Team C","Team D"
    };

    public bool IsPasswordHidden { get; set; } = true;
    public string EyeIcon => IsPasswordHidden ? "eye_closed.png" : "eye_open.png";

    public ICommand TogglePasswordVisibilityCommand => new Command(() =>
    {
        IsPasswordHidden = !IsPasswordHidden;
        OnPropertyChanged(nameof(IsPasswordHidden));
        OnPropertyChanged(nameof(EyeIcon));
        ViewPassword.Source = EyeIcon;
    });
    public UserRegister()
	{
		InitializeComponent();
        NurseTeamPicker.ItemsSource = NurseTeam;
		NurseTeamPicker.SelectedIndex = 0; // Default to the first team
		ViewPassword.Source = EyeIcon; // Set initial icon for password visibility
		//ViewPassword.GestureRecognizers.Add(new TapGestureRecognizer
		//{
		//	Command = TogglePasswordVisibilityCommand
		//});
		// Set the binding context for the page
		BindingContext = this;
    }

	public void OnRegisterNurseClicked(object sender, EventArgs e)
	{
		var username = UsernameEntry.Text;
		var password = PasswordEntry.Text;
		var confirmPassword = ConfirmPasswordEntry.Text;
		//UserStore UserStore = new UserStore();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
		{
			DisplayAlert("Error", "All fields are required.", "OK");
			return;
		}
		if (password != confirmPassword)
		{
			DisplayAlert("Error", "Passwords do not match.", "OK");
			return;
		}
		if (UserStore.IsRegistered(username))
		{
			DisplayAlert("Error", "Username already Exist!!", "OK");
			return;
		}
		try
		{
            TeamName selectedTeam = (TeamName)NurseTeamPicker.SelectedIndex;
			var nurse = new Nurse
			{
				//Id = Guid.NewGuid().ToString(), // Assuming Nurse.Id is the same as the username
				FirstName = NurseFirstName.Text,
				LastName = NurseLastName.Text,
				NickName = NurseNickName.Text,
				EmployeeId = NurseEmployeeId.Text,
				Department = NurseDepartment.Text,
				TeamName = selectedTeam
			};
			
			UpdateTeamInfo updateTeamInfo = new UpdateTeamInfo
			{
				TeamName = selectedTeam,
				NurseId = nurse.Id, // Using Nurse.Id as NurseId
				UpdateDateTime = DateTime.Now
			};

			var userLogin = new UserLogin
			{
				Username = username,
				Password = password,
				PersonId = nurse.Id, // Using Nurse.Id as NurseId
				Role = (int)UserRole.Nurse // Default to Nurse role
			};

			var contactAddress = new ContactAddress
			{
				Id = nurse.Id,
				Address1 = NurseAddress1.Text,
				Address2 = NurseAddress2.Text,
				PhoneNumber = NursePhoneNumber.Text,
				Email = NurseEmail.Text
            };

            if (UserStore.Register(userLogin, nurse, updateTeamInfo,contactAddress))
			{
				DisplayAlert("Success", "Registration successful!", "OK");
				Navigation.PopAsync();
            }
		}
		catch (Exception ex)
		{
			DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            // Optionally log the exception or handle it as needed
        }

       
    }
}