using DroneMonitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DroneMonitoring.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PilotPage : Page
    {
        Gamepad controller;
        DispatcherTimer dispatcherTimer;
        TimeSpan period = TimeSpan.FromMilliseconds(100);
        PilotViewModel vm;
        float Pitch;
        float Roll;
        float Yaw;
        float Throttle;
        public PilotPage()
        {

            this.InitializeComponent();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();

            //public static event EventHandler<Gamepad> GamepadAdded
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            //public static event EventHandler<Gamepad> GamepadRemoved
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            //public event TypedEventHandler<IGameController, Headset> HeadsetConnected
            vm = new PilotViewModel();
            DataContext = vm;
        }

        #region EventHandlers

        private async void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            e.HeadsetConnected += E_HeadsetConnected;
            e.HeadsetDisconnected += E_HeadsetDisconnected;
            e.UserChanged += E_UserChanged;
            await Log("Gamepad Added");
        }

        private async void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            await Log("Gamepad Removed");
        }
        private async void E_UserChanged(IGameController sender, Windows.System.UserChangedEventArgs args)
        {
            await Log("User changed");
        }

        private async void E_HeadsetDisconnected(IGameController sender, Headset args)
        {
            await Log("HeadsetDisconnected");
        }

        private async void E_HeadsetConnected(IGameController sender, Headset args)
        {
            await Log("HeadsetConnected");
        }

        #endregion


        private void dispatcherTimer_Tick(object sender, object e)
        {
            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();

                pbLeftThumbstickX.Value = reading.LeftThumbstickX;
                pbLeftThumbstickY.Value = reading.LeftThumbstickY;

                pbRightThumbstickX.Value = reading.RightThumbstickX;
                pbRightThumbstickY.Value = reading.RightThumbstickY;

                pbLeftTrigger.Value = reading.LeftTrigger;
                pbRightTrigger.Value = reading.RightTrigger;

                //https://msdn.microsoft.com/en-us/library/windows/apps/windows.gaming.input.gamepadbuttons.aspx
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.A), lblA);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.B), lblB);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.X), lblX);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.Y), lblY);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.Menu), lblMenu);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadLeft), lblDPadLeft);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadRight), lblDPadRight);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadUp), lblDPadUp);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadDown), lblDPadDown);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.View), lblView);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.RightThumbstick), ellRightThumbstick);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.LeftThumbstick), ellLeftThumbstick);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.LeftShoulder), rectLeftShoulder);
                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.RightShoulder), recRightShoulder);

                if (reading.Buttons.HasFlag(GamepadButtons.Menu))
                {
                    if(vm.StartTakeoff.CanExecute(null))
                        vm.StartTakeoff.Execute(null);
                }
                if (reading.Buttons.HasFlag(GamepadButtons.View))
                {
                    if (vm.StartLanding.CanExecute(null))
                        vm.StartLanding.Execute(null);
                }
                Pitch = (float) reading.LeftThumbstickY;
                Roll = (float)reading.LeftThumbstickX;
                Yaw = (float)reading.RightThumbstickX;
                Throttle = (float)reading.RightThumbstickY;

                if (vm.Pitch != Pitch || vm.Roll != Roll || vm.Yaw != Yaw || vm.Throttle != Throttle)
                {
                    vm.Pitch = Pitch;
                    vm.Roll = Roll;
                    vm.Yaw = Yaw;
                    vm.Throttle = Throttle;
                    if (vm.MoveAirCraft.CanExecute(null))
                    {
                        vm.MoveAirCraft.Execute(null);
                    }
                }
            }

        }

        #region Helper methods
        private void ChangeVisibility(bool flag, UIElement elem)
        {
            if (flag)
            { elem.Visibility = Visibility.Visible; }
            else
            { elem.Visibility = Visibility.Collapsed; }
        }

        private async Task Log(String txt)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                txtEvents.Text = DateTime.Now.ToString("hh:mm:ss.fff ") + txt + "\n" + txtEvents.Text;
            }
            );

        }
        #endregion

    }
}
