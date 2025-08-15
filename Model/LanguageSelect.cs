using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseCare.Model
{
    
    public class LanguageSelect
    {        
        public static string Title { get; set; } = "Bed Turn Alert";
        public static string Username { get; set; } = "Username";
        public static string Password { get; set; } = "Password";
        public static string Login { get; set; } = "Login";
        public static string RememberMe { get; set; } = "Remember Me";
        public static string PrivacyPolicy { get; set; } = "Privacy Policy";
        public static string Register { get; set; } = "Register";
        public static string ForgotPassword { get; set; } = "Forgot Password?";
        public static string ChangeLanguage { get; set; } = "Change Language";
        public static string FirstName { get; set; } = "First Name";
        public static string LastName { get; set; } = "Last Name";
        public static string Nickname { get; set; } = "Nickname";
        public static string Team { get; set; } = "Team";
        public static string Email { get; set; } = "Email";
        public static string PhoneNumber { get; set; } = "Phone Number";
        public static string PasswordConfirm { get; set; } = "Confirm Password";
        public static string EmployeeID { get; set; } = "Employee ID";
        public static string Department { get; set; } = "Department";
        public static string PatientHN { get; set; } = "Patient HN";
        public static string BedNumber { get; set; } = "Bed Number";
        public static string Disease { get; set; } = "Disease";
        public static string Alert { get; set; } = "Alert";
        public static string DateofBirth { get; set; } = "Date of Birth";
        public static string AdmissionDate { get; set; } = "Admission Date";
        public static string DischargeDate { get; set; } = "Discharge Date";
        public static string Age { get; set; } = "Age";
        public static string ContactPerson { get; set; } = "Contact Person";
        public static string ContactPhone { get; set; } = "Contact Phone";
        public static string TurningTime { get; set; } = "Turning Time";
        public static string TurningInterval { get; set; } = "Turning Interval";
        public static string Hour { get; set; } = "Hour";
        public static string NextTurning { get; set; } = "Next Turning";
        public static string Save { get; set; } = "Save";
        public static string Cancel { get; set; } = "Cancel";
        public static string Nurse { get; set; } = "Nurse";
        public static string Language { get; set; } = "English";
        public static string ChangeBed { get; set; } = "Change Bed";
        public static string PatientRegister { get; set; } = "Patient Register";
        public static string NurseRegister { get; set; } = "Nurse Register";
        public static string Bedridden { get; set; } = "Patient Bedridden";
        public static string TurnBed { get; set; } = "Turn Bedridden";
        public static void SetLanguage(string language)
        {
            switch (language)
            {
                case "en":
                    Title = "Bed Turn Alert";
                    Username = "Username";
                    Password = "Password";
                    Disease = "Disease";
                    Login = "Login";
                    RememberMe = "Remember Me";
                    PrivacyPolicy = "Privacy Policy";
                    Register = "Register";
                    FirstName = "First Name";
                    LastName = "Last Name";
                    Nickname = "Nickname";
                    Team = "Team";
                    Email = "Email";
                    PhoneNumber = "Phone Number";
                    PasswordConfirm = "Confirm Password";
                    EmployeeID = "Employee ID";
                    Department = "Department";
                    PatientHN = "Patient HN";
                    BedNumber = "Bed Number";
                    Alert = "Alert";
                    DateofBirth = "Date of Birth";
                    AdmissionDate = "Admission Date";
                    DischargeDate = "Discharge Date";
                    Age = "Age";
                    ContactPerson = "Contact Person";
                    ContactPhone = "Contact Phone";
                    TurningTime = "Turning Time";
                    TurningInterval = "Turning Interval";
                    Hour = "Hour";
                    NextTurning = "Next Turning";
                    Save = "Save";
                    Cancel = "Cancel";
                    Nurse = "Nurse";
                    Language = "English";
                    ChangeBed = "Change Bed";
                    PatientRegister = "Patient Register";
                    NurseRegister = "Nurse Register";
                    Bedridden = "Patient Bedridden";
                    TurnBed = "Turn Bedridden";
                    break;
                case "th":
                    Title = "เตือนการพลิกตัวผู้ป่วย";
                    Username = "ชื่อผู้ใช้";
                    Password = "รหัสผ่าน";
                    Disease = "โรค";
                    Login = "เข้าสู่ระบบ";
                    RememberMe = "จดจำฉัน";
                    PrivacyPolicy = "นโยบายความเป็นส่วนตัว";
                    Register = "ลงทะเบียน";
                    FirstName = "ชื่อ";
                    LastName = "นามสกุล";
                    Nickname = "ชื่อเล่น";
                    Team = "ทีม";
                    Email = "อีเมล";
                    PhoneNumber = "หมายเลขโทรศัพท์";
                    PasswordConfirm = "ยืนยันรหัสผ่าน";
                    EmployeeID = "รหัสพนักงาน";
                    Department = "แผนก";
                    PatientHN = "รหัสผู้ป่วย";
                    BedNumber = "หมายเลขเตียง";
                    Alert = "เตือน";
                    DateofBirth = "วันเกิด";
                    AdmissionDate = "วันที่เข้ารับการรักษา";
                    DischargeDate = "วันที่ออกจากโรงพยาบาล";
                    Age = "อายุ";
                    ContactPerson = "ผู้ติดต่อ";
                    ContactPhone = "เบอร์โทรศัพท์ผู้ติดต่อ";
                    TurningTime = "เวลาพลิกตัว";
                    TurningInterval = "ช่วงเวลาพลิกตัว";
                    Hour = "ชั่วโมง";
                    NextTurning = "การพลิกตัวถัดไป";
                    Save = "บันทึก";
                    Cancel = "ยกเลิก";
                    Nurse = "พยาบาล";
                    Language = "ไทย";
                    ChangeBed = "เปลี่ยนเตียง";
                    PatientRegister = "ลงทะเบียนผู้ป่วย";
                    NurseRegister = "ลงทะเบียนพยาบาล";
                    Bedridden = "ผู้ป่วยนอนติดเตียง";
                    TurnBed = "พลิกตัวผู้ป่วยนอนติดเตียง";
                    break;
          
                default:
                    Title = "NurseCare"; // Default to English
                    break;
            }
        }
    }
}
