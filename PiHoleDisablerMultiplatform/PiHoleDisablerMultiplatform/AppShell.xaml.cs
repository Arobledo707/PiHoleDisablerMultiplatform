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
        }

        private async void DataEntered()
        {
            CurrentPiData.piHoleData = await PiholeDataSerializer.DeserializeData();
            if (CurrentPiData.piHoleData.Token != null && CurrentPiData.piHoleData.Token != String.Empty)
            {
                await Shell.Current.GoToAsync("//DisablePage");
            }
        }
    }
}
