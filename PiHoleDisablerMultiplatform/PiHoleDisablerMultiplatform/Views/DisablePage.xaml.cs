﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisablePage : ContentPage
    {
        private readonly string piHoleStatusStartString = "Pi-hole status: ";
        private readonly string enabled = "enabled";
        private readonly string disconnected = "disconnected";

        public DisablePage()
        {
            InitializeComponent();
            this.BindingContext = new DisableViewModel();
            MessagingCenter.Subscribe<DisableViewModel, string>(this, Constants.statusUpdate, async (sender, status) =>
            {
                ChangeStatus(status);
            });

        }

        protected void ChangeStatus(string status) 
        {
            piHoleStatusText.Text = piHoleStatusStartString + status;
            if (status == enabled || status == disconnected)
            {
                disableGrid.IsVisible = true;
                enableData.IsVisible = false;
            }
            else 
            {
                disableGrid.IsVisible = false;
                enableData.IsVisible = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.refresh);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
           var result = await DisplayActionSheet("Choose Theme" ,"cancel", null, 
                Constants.Theme.Default.ToString(), Constants.Theme.Blue.ToString(), Constants.Theme.Green.ToString(), Constants.Theme.Orange.ToString());
        }
    }
}