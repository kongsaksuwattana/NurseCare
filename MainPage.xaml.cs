using NurseCare.DataAccess;
using Plugin.LocalNotification;

namespace NurseCare
{
    public partial class MainPage : ContentPage
    {
       // int count = 0;

        public MainPage()
        {
            InitializeComponent();
            this.SizeChanged += RegisterPage_SizeChanged;
            TurningMonitor.StopMonitoring();
            TurningMonitor.StartMonitoring();
            
        }
        private void RegisterPage_SizeChanged(object sender, EventArgs e)
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
        public void OnRegisterNurseClicked(object sender, EventArgs e)
        {
            // Navigate to the RegisterNurse page
            Navigation.PushAsync(new NurseRegister());
        }
        public void OnRegisterPatientClicked(object sender, EventArgs e)
        {
            // Navigate to the RegisterPatient page
            Navigation.PushAsync(new PatientPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var notification = new NotificationRequest
            {
                NotificationId = 1000 + 11,
                Title = "Test Notification",
                Description = $"พลิกตัวผู้ป่วยเตียง เวลา {DateTime.Now:dd-MM-yyyy hh:mm:ss}",
                //ReturningData = "TurningReminder",
                //BadgeNumber = 1,
                CategoryType = NotificationCategoryType.Alarm,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyAutoCancelTime = DateTime.Now.AddMinutes(10), // Auto cancel after 5 minutes
                    RepeatType = NotificationRepeat.TimeInterval,
                    NotifyRepeatInterval = TimeSpan.FromMinutes(1), // Repeat every minute
                   
                    // Show notification immediately for testing
                },
                //NotifyTime = DateTime.Now
            };
            //LocalNotificationCenter.Current.Clear(); // Clear previous notifications
#if ANDROID
           
#endif
            LocalNotificationCenter.Current.Show(notification);
        }
        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
    }

}
