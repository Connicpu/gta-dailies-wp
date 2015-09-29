using GtaLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundNotifier
{
    public sealed class Notifier : IBackgroundTask
    {
        private void SendNotification(string text)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            XmlNodeList elements = toastXml.GetElementsByTagName("text");
            foreach (IXmlNode node in elements)
            {
                node.InnerText = text;
            }

            var xml = toastXml.GetXml();

            ToastNotification notification = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private void UpdateTile(TimeSpan timeLeft, Logic logic, State state)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();

            if (logic.HasCompletedToday(state))
            {
                return;
            }

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);
            tileXml.GetElementsByTagName("text")[0].InnerText =
                string.Format("You have {0} hour{1}!", timeLeft.Hours, timeLeft.Hours == 1 ? "" : "s");

            updater.Update(new TileNotification(tileXml));
        }

        private async Task UpdateToast(TimeSpan timeLeft, Logic logic, State state)
        {
            if (logic.ShouldNotify(state))
            {
                if (timeLeft.Hours >= 2)
                {
                    SendNotification(string.Format("You have {0} hours!", timeLeft.Hours));
                }
                else
                {
                    SendNotification(string.Format("You only have {0:hh\\:mm}!", timeLeft));
                }

                logic.OnNotify(state);
                await state.Save();
            }
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                var logic = new Logic();
                var state = await State.Load();
                var timeLeft = logic.GetTimeUntilExpiration();

                UpdateTile(timeLeft, logic, state);
                await UpdateToast(timeLeft, logic, state);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to notify {0}", ex);
            }

            deferral.Complete();
        }
    }
}
