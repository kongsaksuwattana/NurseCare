using NurseCare.Model;
using NurseCare.DataAccess;

namespace NurseCare;

public partial class UpdateTeamView : ContentView
{
	string[] NurseTeam = TeamName.GetNames(typeof(TeamName));
    string nurseId = Preferences.Get("personId", string.Empty);
    List<Nurse> nurseList = new List<Nurse>();
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
    }

    private void AllTeamCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        
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
}