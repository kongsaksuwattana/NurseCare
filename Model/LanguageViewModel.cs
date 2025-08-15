using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NurseCare.Model;

namespace NurseCare.Model
{    

    public class LanguageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title => LanguageSelect.Title;
        public string Username => LanguageSelect.Username;
        public string Password => LanguageSelect.Password;
        public string Login => LanguageSelect.Login;
        public string RememberMe => LanguageSelect.RememberMe;
        public string PrivacyPolicy => LanguageSelect.PrivacyPolicy;
        public string Register => LanguageSelect.Register;
        public string loginButton => LanguageSelect.Login;
        public string ForgotPassword => LanguageSelect.ForgotPassword;
        public string FirstName => LanguageSelect.FirstName;
        public string LastName => LanguageSelect.LastName;
        public string Nickname => LanguageSelect.Nickname;
        public string Team => LanguageSelect.Team;
        public string Email => LanguageSelect.Email;
        public string PhoneNumber => LanguageSelect.PhoneNumber;
        public string PasswordConfirm => LanguageSelect.PasswordConfirm;
        public string EmployeeID => LanguageSelect.EmployeeID;
        public string Department => LanguageSelect.Department;
        public string PatientHN => LanguageSelect.PatientHN;
        public string BedNumber => LanguageSelect.BedNumber;
        public string Disease => LanguageSelect.Disease;
        public string Alert => LanguageSelect.Alert;
        public string DateofBirth => LanguageSelect.DateofBirth;
        public string AdmissionDate => LanguageSelect.AdmissionDate;
        public string DischargeDate => LanguageSelect.DischargeDate;
        public string Age => LanguageSelect.Age;
        public string ContactPerson => LanguageSelect.ContactPerson;
        public string ContactPhone => LanguageSelect.ContactPhone;
        public string TurningTime => LanguageSelect.TurningTime;
        public string TurningInterval => LanguageSelect.TurningInterval;
        public string Hour => LanguageSelect.Hour;
        public string NextTurning => LanguageSelect.NextTurning;
        public string Save => LanguageSelect.Save;
        public string Cancel => LanguageSelect.Cancel;
        public string Nurse => LanguageSelect.Nurse;    
        public string NurseRegister => $"{LanguageSelect.Nurse} {LanguageSelect.Register}";
        public string Language => LanguageSelect.Language;
        public string Changebed => LanguageSelect.ChangeBed;
        public string PatientRegister => LanguageSelect.PatientRegister;
        public string Bedridden => LanguageSelect.Bedridden;
        public string TurnBed => LanguageSelect.TurnBed;

        // Add other properties as needed...

        public void ChangeLanguage(string lang)
        {
            LanguageSelect.SetLanguage(lang);
            NotifyAllPropertiesChanged();
        }

        private void NotifyAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); // null = notify all
        }
    }
}
