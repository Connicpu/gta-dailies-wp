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
    public sealed partial class DailiesExpire : UserControl
    {
        Logic logic_ = new Logic();
        DispatcherTimer timer_ = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        public DailiesExpire()
        {
            InitializeComponent();
            Update();
            timer_.Tick += delegate
            {
                Update();
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

        void Update()
        {
            var expires_at = logic_.GetExpiration().ToString("MM/dd HH:mm");
            var expires_in = logic_.GetTimeUntilExpiration().ToString("hh\\:mm\\:ss");
            var format = "Dailies expire {0} (in {1})";
            ExpiresText.Text = string.Format(format, expires_at, expires_in);
        }
    }
}
