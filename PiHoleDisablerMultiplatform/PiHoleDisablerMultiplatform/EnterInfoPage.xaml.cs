using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform
{
    public partial class EnterInfoPage : Xamarin.Forms.Shell
    {
        public EnterInfoPage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
        }
    }
}