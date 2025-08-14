using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace NurseCare
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, 
        LaunchMode = LaunchMode.SingleTop, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelId = "turning_channel";
                var channelName = "Turning Reminders";
                var channelDescription = "Reminders to turn patients on schedule";
                var importance = NotificationImportance.High;

                var channel = new NotificationChannel(channelId, channelName, importance)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
            if (Platform.CurrentActivity.CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                Platform.CurrentActivity.RequestPermissions(new[] { Android.Manifest.Permission.PostNotifications }, 0);
            }
        }


        //public void ShowAndroidNotification(string title, string message)
        //{
        //    var builder = new NotificationCompat.Builder(this, "turning_channel")
        //        .SetContentTitle(title)
        //        .SetContentText(message)
        //        //.SetSmallIcon(Resource.Drawable.ic_notification)
        //        .SetPriority(NotificationCompat.PriorityHigh);

        //    var notificationManager = NotificationManagerCompat.From(this);
        //    notificationManager.Notify(1001, builder.Build());
        //}


    }
}
