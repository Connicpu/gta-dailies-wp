using GtaLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GtaDailies.Controls
{
    public sealed partial class Streak : UserControl
    {
        DispatcherTimer timer_ = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };

        public Streak()
        {
            InitializeComponent();
            Update();
            timer_.Tick += delegate
            {
                if (MainPage.Changed)
                {
                    Update();
                    MainPage.Changed = false;
                }
            };
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            timer_.Start();
        }

        void OnUnload(object sender, RoutedEventArgs e)
        {
            timer_.Stop();
        }

        public async void Update()
        {
            var state = await State.Load();
            var logic = new Logic();
            var streak = logic.GetStreak(state);

            StreakValue.Text = string.Format("{0} day{1}", streak, streak != 1 ? "s" : "");
        }
    }
}
