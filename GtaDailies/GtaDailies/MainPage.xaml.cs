using GtaLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace GtaDailies
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static bool Changed = false;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var logic = new Logic();
            var expires = logic.GetExpiration();

            RegisterBackgroundTask();
        }

        private async void RegisterBackgroundTask()
        {
            try
            {
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity ||
                    status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity)
                {
                    bool isRegistered = BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == "Notification task");
                    if (!isRegistered)
                    {
                        BackgroundTaskBuilder builder = new BackgroundTaskBuilder
                        {
                            Name = "Notification task",
                            TaskEntryPoint = "BackgroundNotifier.Notifier"
                        };
                        builder.SetTrigger(new TimeTrigger(30, false));
                        BackgroundTaskRegistration task = builder.Register();
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("The access has already been granted");
            }
        }
    }
}
