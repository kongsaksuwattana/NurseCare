using NurseCare.Model;
using NurseCare.DataAccess;

namespace NurseCare;

public partial class UpdateTeamView : ContentView
{
	string[] NurseTeam = TeamName.GetNames(typeof(TeamName));
    string nurseId = Preferences.Get("personId", string.Empty);
    List<Nurse> nurseList = new List<Nurse>();
    List<Bed> bedList = new List<Bed>();
    NurseCareDataQuery dataQuery = new NurseCareDataQuery();
    NurseCareDBContext dbContext = new NurseCareDBContext();
    Nurse nurse;
    UpdateBedInfo updateBedInfo;
    public UpdateTeamView()
	{
		InitializeComponent();        
        //TeamPickerFrom.ItemsSource = NurseTeam;
        TeamPickerTo.ItemsSource = NurseTeam;
        TeamPickerTo.SelectedIndex = 0; // Default to the first team

        TeamSupportBed.ItemsSource = NurseTeam;
        TeamSupportBed.SelectedIndex = 0;
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (nurse != null)
        {
            try
            {
                nurse.TeamName = (TeamName)TeamPickerTo.SelectedIndex;
                UpdateTeamInfo teamInfo = new UpdateTeamInfo
                {
                    TeamName = (TeamName)TeamPickerTo.SelectedIndex,
                    NurseId = nurse.Id,
                    UpdateDateTime = DateTime.Now
                };
                dbContext.Nurses.Update(nurse);
                dbContext.UpdateTeamInfos.Add(teamInfo);
                dbContext.SaveChanges();
                TeamPickerFrom.Text = nurse.TeamName.ToString();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Update team error: {ex.Message}", "OK");
            }
        }
    }
	private void AddBedButton_Clicked(object sender, EventArgs e)
	{
        // Logic to add a new bed to the team
        if(string.IsNullOrEmpty( BedIdEntry.Text))
        {
            Application.Current.MainPage.DisplayAlert("Error", "กรุณากรอกหมายเลขเตียง", "OK");
            return;
        }
        if(TeamSupportBed.SelectedIndex < 0)
        {
            Application.Current.MainPage.DisplayAlert("Error", "กรุณาเลือกทีมที่สนับสนุนเตียง", "OK");
            return;
        }
        string bedId = BedIdEntry.Text.Trim();
        if (bedList.Any(b => b.BedId == bedId))
        {
            Application.Current.MainPage.DisplayAlert("Error", "เตียงนี้มีอยู่แล้วในระบบ", "OK");
            return;
        }
        Bed newBed = new Bed
        {
            BedId = bedId,
            TeamSupport = (TeamName)TeamSupportBed.SelectedIndex,
            IsOccupied = false // Assuming new beds are not occupied by default
        };
        try
        {
            dbContext.Beds.Add(newBed);
            dbContext.SaveChanges();
            Application.Current.MainPage.DisplayAlert("Success", "เพิ่มเตียงใหม่เรียบร้อยแล้ว", "OK");
            // Refresh the bed list
            bedList = dataQuery.GetBeds();
            var bedIdList = bedList.Select(b => b.BedId).ToArray();
            BedPicker.ItemsSource = bedIdList;
            BedPicker.SelectedIndex = -1; // Reset selection
            BedIdEntry.Text = string.Empty; // Clear input field
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", $"เพิ่มเตียงไม่สำเร็จ: {ex.Message}", "OK");
        }
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        nurseId = Preferences.Get("personId", string.Empty);
        nurse = dataQuery.GetNursebyId(nurseId);
        //var teamInfo = dataQuery.GetTeamInfoById(nurseId);
        nurseList = dataQuery.GetNurses();
        //TeamPickerFrom.SelectedIndex = nurse?.TeamName != null ? Array.IndexOf(NurseTeam, nurse.TeamName.ToString()) : 0;
        TeamPickerFrom.Text = nurse?.TeamName != null ? nurse.TeamName.ToString() : string.Empty;
        if (nurseList != null && nurse != null)
        {
            var nurseName = nurseList?.Select(n => $"{n.FirstName} {n.LastName}").ToArray();
            NursePicker.ItemsSource = nurseName;
            NursePicker.SelectedIndex = nurseList.IndexOf(nurseList.FirstOrDefault(n => n.Id == nurse.Id));
        }

        bedList = dataQuery.GetBeds();
        if (bedList != null)
        {
            var bedIdList = bedList.Select(b => b.BedId).ToArray();
            BedPicker.ItemsSource = bedIdList;
            BedPicker.SelectedIndex = -1; // Reset selection
        }
        else
        {
            BedPicker.ItemsSource = new string[0]; // No beds available
        }
    }

    private void AllTeamCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        
    }

    private void BedPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BedPicker.SelectedIndex >= 0)
        {
            string selectedBedId = BedPicker.SelectedItem.ToString();
            var selectedBed = bedList.FirstOrDefault(b => b.BedId == selectedBedId);
            if (selectedBed.IsOccupied)
            {
                updateBedInfo = dataQuery.GetUpdateBedInfoByBedId(selectedBedId);
                if (updateBedInfo != null)
                {
                    ResultLabel.Text = $"เตียง {selectedBedId} ถูกใช้งานโดย {updateBedInfo.PatientHN}\n" +
                                      $"พยาบาล: {updateBedInfo.NurseId}\n" +
                                      $"เวลาการเปลี่ยนท่าล่าสุด: {updateBedInfo.TurningTime}\n" +
                                      $"เวลาการเปลี่ยนท่าถัดไป: {updateBedInfo.NextTurningTime}";
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Error", "ไม่พบข้อมูลการอัพเดตเตียงนี้", "OK");
                }
            }
            else
            {
                ResultLabel.Text = $"เตียง {selectedBedId} ยังว่างอยู่";
            }
        }
        else ResultLabel.Text = string.Empty;
    }

    private void NursePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        string[] nurseName = NursePicker.SelectedItem?.ToString().Split(" ")?? new string[2];
        nurse = nurseList.FirstOrDefault(n => (n.FirstName??string.Empty)==nurseName[0] && (n.LastName??string.Empty)==nurseName[1]);
        TeamPickerFrom.Text = nurse?.TeamName != null ? nurse.TeamName.ToString() : string.Empty;
        if (nurse != null)
        {
            //var nurseNameList = nurseList?.Select(n => $"{n.FirstName} {n.LastName}").ToArray();
            //NursePicker.ItemsSource = nurseNameList;
            NursePicker.SelectedIndex = nurseList.IndexOf(nurseList.FirstOrDefault(n => n.Id == nurse.Id));
        }
    }

    private async void DelNurse_Clicked(object sender, EventArgs e)
    {
       var result = await Application.Current.MainPage.DisplayAlert("Warning", $"ต้องการลบข้อมูลพยาบาล {nurse.FirstName} {nurse.LastName} ใช่หรือไม่", "Yes","No");
        if(result)
        {
            Person? person = dbContext.Persons.FirstOrDefault(p => p.Id == nurse.Id);
            ContactAddress? contactAddress = dbContext.ContactAddresses.FirstOrDefault(c => c.Id == nurse.Id);
            UserLogin? userLogin = dbContext.UserLogins.FirstOrDefault(u => u.PersonId == nurse.Id);
            string removeId = nurse.Id;
            if (person != null)
            {
                dbContext.Persons.Remove(person);
            }
            if (contactAddress != null)
            {
                dbContext.ContactAddresses.Remove(contactAddress);
            }
            if (userLogin != null)
            {
                dbContext.UserLogins.Remove(userLogin);
            }
            dbContext.Nurses.Remove(nurse);
            try
            {
                dbContext.SaveChanges();
                await Application.Current.MainPage.DisplayAlert("Success", "ลบข้อมูลพยาบาลเรียบร้อยแล้ว", "OK");
                if(removeId == nurseId)
                {
                    Preferences.Remove("personId"); // Remove the current nurse ID from preferences
                    Application.Current.MainPage = new NavigationPage(new LoginPage()); // Navigate to login page
                }
                // Refresh the nurse list or navigate back
                nurseList = dataQuery.GetNurses();
                NursePicker.ItemsSource = nurseList.Select(n => $"{n.FirstName} {n.LastName}").ToArray();
                NursePicker.SelectedIndex = -1; // Reset selection
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"ลบข้อมูลพยาบาลไม่สำเร็จ: {ex.Message}", "OK");
            }
        }
    }
}