using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.Models;
using System.Threading.Tasks;


namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class DisableViewModel : BaseViewModel
    {
        private PiHoleData pihole;
        public Command DisableCommand { get; }
        public Command EnableCommand { get; }
        public DisableViewModel() 
        {
            Title = "Disable";
            DisableCommand = new Command(OnDisableButtonClicked);
            EnableCommand = new Command(OnEnableButtonClicked);

            LoadData();

            MessagingCenter.Subscribe<DisablePage>(this, "refresh", async (sender) =>
            {
                string result = await PiholeHttp.CheckPiholeStatus(pihole.Url, pihole.Token);
                MessagingCenter.Send(this, "statusupdate", result);

            });
        }

        private bool LoadData() 
        {
            return true;
        }
        private async Task<bool> Refresh() 
        {
            await PiholeHttp.CheckPiholeStatus(pihole.Url, pihole.Token);
            return await Task.FromResult(true); 
        }

        private async void OnDisableButtonClicked(object obj) 
        {

        }

        private async void OnEnableButtonClicked() 
        {

        }
    }
}