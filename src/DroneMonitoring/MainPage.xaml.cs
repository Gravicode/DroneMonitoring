using DJI.WindowsSDK;
using DroneMonitoring.Helpers;
using DroneMonitoring.Services;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DroneMonitoring
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private struct SDKModuleSampleItems
        {
            public String header;
            public List<KeyValuePair<String, Type>> items;
        }

        private List<SDKModuleSampleItems> navigationModules = new List<SDKModuleSampleItems>
        {
            new SDKModuleSampleItems() {
                header = "Activation", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Activating DJIWindowsSDK", typeof(DJISDKInitializing.ActivatingPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "FPV", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("FPV", typeof(FPV.FPVPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Component Handling", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Component Handling", typeof(ComponentHandling.ComponentHandingPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Waypoint", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Using Simulator", typeof(WaypointHandling.SimulatorPage)),
                    new KeyValuePair<string, Type>("Waypoint Mission", typeof(WaypointHandling.WaypointMissionPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Account", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Account Management", typeof(UserAccount.UserAccountPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Flysafe", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Flyzone", typeof(Flysafe.FlyzonePage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Playback", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Playback", typeof(Playback.PlaybackPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Pilot", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Pilot", typeof(Pages.PilotPage)),
                },
            },

        };

        public MainPage()
        {
            this.InitializeComponent();
            
            var module = navigationModules[0];
            NavView.MenuItems.Add(new NavigationViewItemHeader() { Content = module.header });
            foreach (var item in module.items)
            {
                NavView.MenuItems.Add(item.Key);
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            String invokedName = args.InvokedItem as String;
            foreach (var module in navigationModules)
            {
                foreach (var item in module.items)
                {
                    if (invokedName == item.Key)
                    {
                        if (ContentFrame.SourcePageType != item.Value)
                        {
                            ContentFrame.Navigate(item.Value);
                        }
                        return;
                    }
                }
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            //DJISDKManager.Instance.SDKRegistrationStateChanged += Instance_SDKRegistrationEvent;
            var reg = ServiceContainer.Instance.Resolve<SDKRegistrationService>();
            reg.StateChanged += async (a, ev) => {
                if (ev.Result == SDKError.NO_ERROR)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        for (int i = 1; i < navigationModules.Count; ++i)
                        {
                            var module = navigationModules[i];
                            NavView.MenuItems.Add(new NavigationViewItemHeader() { Content = module.header });
                            foreach (var item in module.items)
                            {
                                NavView.MenuItems.Add(item.Key);
                            }
                        }
                        var selitem = navigationModules.Last();
                        ContentFrame.Navigate(selitem.items.First().Value);
                    });
                }
            };
            if (!reg.IsActive)
            {
                reg.Register(AppConstants.AppKey);
            }
        }

        
    }
}
