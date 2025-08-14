using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NurseCare.Model;
using Plugin.LocalNotification;

namespace NurseCare.DataAccess
{
    public static class TurningMonitor
    {
        private static CancellationTokenSource _cts;
        private static NurseCareDataQuery dataQuery = new NurseCareDataQuery();
        public static NotificationRequest notification { get; set; } = new NotificationRequest();
        public static void StartMonitoring()
        {
            _cts = new CancellationTokenSource();
            string teamSupport = Preferences.Get("team", string.Empty);
            if (string.IsNullOrEmpty(teamSupport))
            {
                throw new InvalidOperationException("Team support is not set in preferences.");
            }
            TeamName team = (TeamName)Enum.Parse(typeof(TeamName), teamSupport);
            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    //DateTime now = DateTime.Now;

                    //var nextTurn = dataQuery.GetallNotify().FirstOrDefault(t => t.NextTurningTime > now);

                    //if (nextTurn != default && (nextTurn.NextTurningTime - now)?.TotalMinutes <= 1)
                    //{
                    //    ShowNotification("Turning Reminder", $"It's time to turn the patient at {nextTurn:t}");
                    //}
                    
                    List<UpdateBedInfoAndEffect?>? nextTurns = dataQuery.GetallNotify()?
                        .Where(t => t.TeamSupport == team && t.NextTurningTime != null)
                        .ToList();
                    ShowNotification("Turning Reminder", "",nextTurns);
                    await Task.Delay(TimeSpan.FromMinutes(2), _cts.Token);
                }
            });
        }

        public static void StopMonitoring()
        {
            _cts?.Cancel();
        }
        public static void ShowNotification(string title, string? message,List<UpdateBedInfoAndEffect?>? notifyTimes)
        {
            notification = new NotificationRequest();
            if (notifyTimes != null)
            {

                foreach (var bedinfo in notifyTimes)
                {
                    notification = new NotificationRequest
                    {
                        NotificationId = 100 + bedinfo.updateId,
                        Title = title,
                        Description = $"พลิกตัวผู้ป่วยเตียง {bedinfo.BedId} เวลา {bedinfo.NextTurningTime:dd-MM-yyyy hh:mm:ss}",
                        ReturningData = "TurningReminder",
                        CategoryType = NotificationCategoryType.Reminder,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = bedinfo.NextTurningTime,
                            NotifyAutoCancelTime = bedinfo.NextTurningTime?.AddMinutes(15), // Auto cancel after 5 minutes
                            RepeatType = NotificationRepeat.TimeInterval,
                            NotifyRepeatInterval = TimeSpan.FromMinutes(1) // Repeat every minute

                            // Show notification immediately for testing
                        },
                        //NotifyTime = DateTime.Now
                    };
                }
            }
            LocalNotificationCenter.Current.CancelAll(); // Clear previous notifications
            if(notification != new NotificationRequest())
            {
                LocalNotificationCenter.Current.Show(notification);
            }
            
            //NotificationCenter.Current.Show(notification);

        }
    }    
}