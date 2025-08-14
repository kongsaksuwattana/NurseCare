using Foundation;
using NurseCare.Platforms.iOS;
using UIKit;
using UserNotifications;

namespace NurseCare
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UNUserNotificationCenter.Current.Delegate = new MyNotificationDelegate();

            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound,
                (approved, err) => {
                    // Handle approval
                });
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
            return base.FinishedLaunching(app, options);
        }

    }
}

    
