using FluentFTP;
using Microsoft.Maui.Media;
using NurseCare.DataAccess;
using NurseCare.Model;
using System.Diagnostics;
using System.IO;

namespace NurseCare;

public partial class TurnBedriddenView : ContentView
{
    SearchHNView SearchHNView = new SearchHNView();
    NurseCareDataQuery dataQuery = new NurseCareDataQuery();    
    Patient? selectedPatient;   
    TurnPosture selectedPosture = TurnPosture.Left; // Default posture
    byte[] photoBytes;
    string imageUrl = string.Empty;
    public TurnBedriddenView()
    {
        InitializeComponent();
        SearchViewGrid.Children.Add(SearchHNView);
        SearchHNView.PatientSelected += SearchHNView_PatientSelected;
    }

    private void SearchHNView_PatientSelected(object? sender, Patient? e)
    {
        if (e != null)
        {
            ChangeBedSwitch.IsEnabled = true;
            try
            {
                selectedPatient = e;
                // Display patient details or update UI as needed
                string nickname = string.IsNullOrEmpty(e.NickName) ? string.Empty : $"({e.NickName})";
                PatientNameEntry.Text = $"{e.FirstName} {e.LastName} {nickname}";
                AgeEntry.Text = e.DateOfBirth.HasValue ? DateTime.Today - e.DateOfBirth.Value < TimeSpan.FromDays(1) ? "0 ปี" :
                    $"อายุ {DateTime.Now.Year - e.DateOfBirth.Value.Year} ปี" : "ไม่ระบุ";
                DiseaseEntry.Text = e.Disease;
                BedIDEntry.Text = e.BedId;

               
                TurnDate.Date = DateTime.Now.Date;
                TurnTime.Time = DateTime.Now.TimeOfDay;
                //DateTime nextDT = DateTime.Now.AddHours(int.Parse(RepeatIntervalEntry.Text));
                //NextTurnDate.Date = nextDT.Date;
                //NextTurnTime.Time = nextDT.TimeOfDay;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error displaying patient details: {ex.Message}");
                Application.Current.MainPage.DisplayAlert("Error", $"Failed to display patient details {ex.Message}", "OK");
            }
        }
        else ChangeBedSwitch.IsEnabled = false; // Disable the switch if no patient is selected
    }
    private void SaveTurnButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            List<UpdateBedInfo>? allnotifybyId = dataQuery.GetallNotifyByPatientId(selectedPatient?.Id);
            
            UpdateBedInfoAndEffect bedInfoAndEffect = new UpdateBedInfoAndEffect()
            {
                PatientId = selectedPatient?.Id,
                PatientHN = selectedPatient?.HN,
                BedId = selectedPatient?.BedId,
                NurseId = Preferences.Get("personId", ""),
                TurningTime = TurnDate.Date.Add(TurnTime.Time),
                NextTurningTime = NextTurnDate.Date.Add(NextTurnTime.Time),
                UpdateDateTime = DateTime.Now,
                IsManualKeyed = !SearchHNView.isFromCamera, // Set to true as this is a manual entry
                ImageUrl = imageUrl,
                IsNotifying = true, // Set to true to indicate that this update is notifying
                TeamSupport = Preferences.Get("team", string.Empty) switch
                {
                    "TeamA" => TeamName.TeamA,
                    "TeamB" => TeamName.TeamB,
                    "TeamC" => TeamName.TeamC,
                    "TeamD" => TeamName.TeamD,
                    _ => null
                },
                Posture = selectedPosture,
                Occiput = OCCIPUT_cb.IsChecked,
                Scapula = SCAPULA_cb.IsChecked,
                Sacrum_Coccyx = SACRUM_cb.IsChecked,
                Heel = HEEL_cb.IsChecked
            };
            using (var db = new NurseCareDBContext())
            {
                db.BedInfoAndEffects.Add(bedInfoAndEffect);
                if (allnotifybyId != null)
                {
                    allnotifybyId.ForEach(a => a.IsNotifying = false);
                    db.UpdateBedInfos.UpdateRange(allnotifybyId);   //Clear all notify this patient
                }
                if (selectedPatient != null)
                    db.Patients.Update(selectedPatient); // Update the patient with the new bed info
                db.SaveChanges();
            }

            List<UpdateBedInfoAndEffect?>? turningTimes = dataQuery.GetallNotify()?
                        .Where(t => t.TeamSupport == bedInfoAndEffect.TeamSupport && t.NextTurningTime != null)
                        .ToList();
            if(turningTimes != null && turningTimes.Count > 0)
            {
                TurningMonitor.ShowNotification("Turning Reminder", string.Empty, turningTimes);
            }
            SearchHNView.isFromCamera = false; // Reset the flag after saving
            // Clear the input fields after saving
            PatientNameEntry.Text = string.Empty;
            AgeEntry.Text = string.Empty;
            DiseaseEntry.Text = string.Empty;
            BedIDEntry.Text = string.Empty;
            TurnDate.Date = DateTime.Now.Date;
            TurnTime.Time = DateTime.Now.TimeOfDay;
            NextTurnDate.Date = DateTime.Now.Date;
            NextTurnTime.Time = DateTime.Now.TimeOfDay;
            OCCIPUT_cb.IsChecked = false;
            SCAPULA_cb.IsChecked = false;
            SACRUM_cb.IsChecked = false;
            HEEL_cb.IsChecked = false;
            CapturedImage.Source = null; // Clear the captured image
            Application.Current.MainPage.DisplayAlert("Success", "บันทึกข้อมูลการพลิกตัวผู้ป่วยเรียบร้อยแล้ว", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating UpdateBedInfoAndEffect: {ex.Message}");
            Application.Current.MainPage.DisplayAlert("Error", $"Failed to create update turn info: {ex.Message}", "OK");
            return;
        }
    }

    private void ChangeBedButton_Clicked(object sender, EventArgs e)
    {

    }

    private async void TakePicture_Clicked(object sender, EventArgs e)
    {       
        var photo = await MediaPicker.Default.CapturePhotoAsync();
        if (photo != null)
        {
            // Display the photo or handle it as needed
            var stream = await photo.OpenReadAsync();
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                photoBytes = memoryStream.ToArray();
            }
            CapturedImage.Source = ImageSource.FromStream(() => new MemoryStream(photoBytes));
            string newFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{selectedPatient?.HN}.png";
            //selectedPatient.ImageUrl = newFileName;
            imageUrl = newFileName;
            await FileFtp.UploadFileAsync(photoBytes, newFileName,"patient");
            //photo = null;
            //await UploadPhotoAsync(photo);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to take picture", "OK");
        }
    }
    


    public async Task<FileResult?> TakePhotoAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported)
            return null;

        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            return photo;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Capture failed: {ex.Message}");
            return null;
        }
    }
        

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        List<Bed> vacantBed = dataQuery.GetVacantBeds();
        if (vacantBed != null && vacantBed.Count > 0)
        {
            ChangedBedIdPicker.ItemsSource = vacantBed.Select(b=>b.BedId).ToArray();
        }
        else
        {
           ChangedBedIdPicker.ItemsSource = null;
        }
    }
    
    private void ChangeBedSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if(ChangeBedSwitch.IsToggled)
        {
            ChangeBedButton.IsVisible = true;
            ChangedBedIdPicker.IsVisible = true;
            ChangedBedIdPicker.SelectedIndex = 0; // Default to the first bed
        }
        else
        {
            ChangeBedButton.IsVisible = false;
            ChangedBedIdPicker.IsVisible = false;
            ChangedBedIdPicker.SelectedIndex = -1; // Reset selection
        }
    }

    private void RepeatIntervalEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        ChangeTurnTime();
    }
    private void TurnDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        ChangeTurnTime();
    }

    private void TurnTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        ChangeTurnTime();
    }
    private void ChangeTurnTime()
    {
        if(TurnDate.Date == null || TurnTime.Time == null || string.IsNullOrEmpty(RepeatIntervalEntry.Text))
        {
            return; // Ensure all fields are set before calculating next turn time
        }
        DateTime nextDT = TurnDate.Date.Add(TurnTime.Time).AddHours(int.Parse(RepeatIntervalEntry.Text));
        NextTurnDate.Date = nextDT.Date;
        NextTurnTime.Time = nextDT.TimeOfDay;
    }

    private void ChangedBedIdPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ChangedBedIdPicker.SelectedIndex> -1)
        {
            selectedPatient.BedId = ChangedBedIdPicker.SelectedItem.ToString();
        }
        
    }

    private void PostureRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = (RadioButton)sender;
        switch (radioButton.Value)
        {
            case 0:
                selectedPosture = TurnPosture.Left;
                break;
            case 1:
                selectedPosture = TurnPosture.Right;
                break;
            case 2:
                selectedPosture = TurnPosture.Supine;
                break;            
            default:
                selectedPosture = TurnPosture.Left; // Default posture
                break;
        }
    }
}