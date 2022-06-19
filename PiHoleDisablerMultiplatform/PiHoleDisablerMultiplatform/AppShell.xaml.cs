using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}
