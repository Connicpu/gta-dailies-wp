using GtaLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
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
    public sealed partial class MarkButton : UserControl
    {
        public MarkButton()
        {
            InitializeComponent();
        }

        private async void OnLoad(object sender, RoutedEventArgs args)
        {
            var state = await State.Load();
            var logic = new Logic();

            if (logic.HasCompletedToday(state))
            {
                DidComplete();
            }
        }

        private async void IDidThem(object sender, RoutedEventArgs args)
        {
            var dialog = new MessageDialog(
                "Don't push this button if you didn't ;)",
                "Are you sure you did them?"
                );
            var ok = new UICommand("I'm sure!");
            var cancel = new UICommand("No I didn't :(");
            dialog.Commands.Add(ok);
            dialog.Commands.Add(cancel);
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            if (result == ok)
            {
                try
                {
                    var state = await State.Load();
                    var logic = new Logic();
                    logic.MarkCompletion(state);
                    await state.Save();
                }
                catch
                {
                    dialog = new MessageDialog(
                        "Something went wrong trying to save your status :(",
                        "Oh noes!"
                        );
                    dialog.Commands.Add(new UICommand("Sad face!"));
                    await dialog.ShowAsync();
                    return;
                }

                DidComplete();
                MainPage.Changed = true;
            }
        }

        private void DidComplete()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();

            Didnt.Visibility = Visibility.Collapsed;
            Did.Visibility = Visibility.Visible;
        }
    }
}
