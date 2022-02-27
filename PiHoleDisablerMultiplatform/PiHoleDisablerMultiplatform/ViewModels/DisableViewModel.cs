﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Models;
using System.Threading.Tasks;


namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class DisableViewModel : BaseViewModel
    {
        public Command DisableCommand { get; }
        public Command EnableCommand { get; }

        private readonly string statusUpdate = "statusUpdate";
        private readonly string refresh = "refresh";

        public DisableViewModel() 
        {
            Title = "Disable";
            DisableCommand = new Command(OnDisableButtonClicked);
            EnableCommand = new Command(OnEnableButtonClicked);

            MessagingCenter.Subscribe<DisablePage>(this, refresh, async (sender) =>
            {
                string result = await PiholeHttp.CheckPiholeStatus(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token);
                MessagingCenter.Send(this, statusUpdate, result);
            });
        }

        private async Task<bool> Refresh() 
        {
            await PiholeHttp.CheckPiholeStatus(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token);
            return await Task.FromResult(true); 
        }

        private async void OnDisableButtonClicked(object obj) 
        {
            string test = obj.ToString();
        }

        private async void OnEnableButtonClicked(object obj) 
        {

        }
    }
}