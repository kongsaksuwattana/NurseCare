using NurseCare.Model;
using NurseCare.DataAccess;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace NurseCare;

public partial class PatientPage : ContentPage
{
    TurnBedriddenView TurnBedriddenView = new TurnBedriddenView();
    NurseCareDataQuery dataQuery = new NurseCareDataQuery();
    const int turningIntervalHours = 2; // Set the turning interval to 2 hours
    public PatientPage()
	{
		InitializeComponent();
		this.SizeChanged += PatientPage_SizeChanged;
        TurnBedridden.Children.Add(TurnBedriddenView);
    }
	private void PatientPage_SizeChanged(object sender, EventArgs e)
	{
        if (Width > Height)
        {
            // Landscape: Patient left, Nurse right
            VisualStateManager.GoToState(MainGrid, "Landscape");
            MainGrid.SetRow(MainGrid.Children[0], 0);
            MainGrid.SetColumn(MainGrid.Children[0], 0);
            MainGrid.SetRow(MainGrid.Children[1], 0);
            MainGrid.SetColumn(MainGrid.Children[1], 1);
        }
        else
        {
            // Portrait: Patient top, Nurse bottom
            VisualStateManager.GoToState(MainGrid, "Portrait");
            MainGrid.SetRow(MainGrid.Children[0], 0);
            MainGrid.SetColumn(MainGrid.Children[0], 0);
            MainGrid.SetRow(MainGrid.Children[1], 1);
            MainGrid.SetColumn(MainGrid.Children[1], 0);
        }
    }

    public void OnRegisterPatientClicked(object sender, EventArgs e)
    {
        Patient patient = new Patient
        {
            HN = PatientHN.Text,
            FirstName = PatientFirstName.Text,
            LastName = PatientLastName.Text,
            DateOfBirth = PatientDOB.Date,
            Disease = PatientDisease.Text,
            BedId = BedIdPicker.SelectedItem?.ToString(),
            DateOfAdmission = PatientAdmissionDate.Date,
            DateOfDischarge = IsDischargeCheck.IsChecked ? PatientDischargeDate.Date : null,
            ContactPerson = PatientContactPhone.Text,
            ContactPhone = PatientContactPhone.Text,
        };
        
        
        if (string.IsNullOrWhiteSpace(patient.HN) || string.IsNullOrWhiteSpace(patient.FirstName) || string.IsNullOrWhiteSpace(patient.LastName))
        {
            DisplayAlert("Error", "กรุณากรอกข้อมูลให้ครบถ้วน_", "OK");
            return;
        }
        if (patient.DateOfAdmission > DateTime.Now)
        {
            DisplayAlert("Error", "วันเข้ารับบริการไม่สามารถบันทึกล่วงหน้าได้", "OK");
            return;
        }
        if (IsDischargeCheck.IsChecked && patient.DateOfDischarge.HasValue && patient.DateOfDischarge.Value < patient.DateOfAdmission)
        {
            DisplayAlert("Error", "วันออกจากโรงพยาบาลต้องไม่ก่อนวันเข้ารับบริการ", "OK");
            return;
        }
        if (patient.DateOfBirth.HasValue && patient.DateOfBirth.Value > DateTime.Now)
        {
            DisplayAlert("Error", "วันเกิดไม่สามารถบันทึกล่วงหน้าได้", "OK");
            return;
        }
        if(BedIdPicker.SelectedIndex == -1)
        {
            DisplayAlert("Error", "กรุณาเลือกเตียงที่ว่าง", "OK");
            return;
        }
        Bed? bed = dataQuery.GetBedById(BedIdPicker.SelectedItem.ToString());
        if (bed == null)
        {
            DisplayAlert("Error", "ไม่พบเตียงที่เลือก", "OK");
            return;
        }
        try
        {
            string teamsuport = Preferences.Get("team", "");
            TeamName? teamName = string.IsNullOrEmpty(teamsuport) ? null : (TeamName)Enum.Parse(typeof(TeamName), teamsuport);
            UpdateBedInfo updateBedInfo = new UpdateBedInfo
            {
                BedId = bed.BedId,
                PatientHN = patient.HN,
                PatientId = patient.Id,
                NurseId = Preferences.Get("personId", ""),
                TurningTime = DateTime.Now,
                NextTurningTime = DateTime.Now.AddHours(turningIntervalHours), // Set next turning time to 2 hours later
                UpdateDateTime = DateTime.Now,
                IsManualKeyed = false // Set to false as this is an automatic entry
            };


            using (var db = new NurseCareDBContext())
            {
                db.Patients.Add(patient);
                bed.IsOccupied = true; // Mark the bed as occupied
                bed.TeamSupport = teamName; // Set the team support from preferences
                db.Update(bed); // Update the bed status
                db.UpdateBedInfos.Add(updateBedInfo); // Add the update bed info
                db.SaveChanges();
            }
            DisplayAlert("Success", "บันทึกข้อมูลผู้ป่วยเรียบร้อยแล้ว", "OK");
            // Clear input fields
            PatientHN.Text = string.Empty;
            PatientFirstName.Text = string.Empty;
            PatientLastName.Text = string.Empty;
            PatientDOB.Date = DateTime.Now;
            PatientDisease.Text = string.Empty;
            PatientAdmissionDate.Date = DateTime.Now;
            IsDischargeCheck.IsChecked = false;
            PatientDischargeDate.Date = DateTime.Now;
            PatientContactPerson.Text = string.Empty;
            PatientContactPhone.Text = string.Empty;


        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"ไม่สามารถบันทึกข้อมูลได้: {ex.Message}", "OK");

        }
    }

    private void IsDischargeCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //IsDischargeCheck.IsChecked? PatientDischargeDate
        if (IsDischargeCheck.IsChecked)
            PatientDischargeDate.IsEnabled = true;
        else PatientDischargeDate.IsEnabled = false;
    }

    private void RegisterVisible_Clicked(object sender, EventArgs e)
    {
        PatientRegView.IsVisible = !PatientRegView.IsVisible;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        List<Bed> beds = dataQuery.GetVacantBeds();
        if(beds.Count > 0)
        {
           BedIdPicker.ItemsSource = beds.Select(b => b.BedId).ToList();
           
            BedIdPicker.SelectedIndex = 0; // Select the first bed by default
        }
        else
        {
           BedIdPicker.ItemsSource = null;
        }
    }
}