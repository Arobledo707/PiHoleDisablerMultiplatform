using System;
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
        public Command ButtonClickCommand { get; }
        public Command RefreshCommand { get; }

        private bool isRefreshing = false;
        public bool IsCurrentlyRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        public DisableViewModel() 
        {
            Title = "Disable";
            ButtonClickCommand = new Command(OnButtonClicked);
            RefreshCommand = new Command(Refresh);

            MessagingCenter.Subscribe<DisablePage>(this, Commands.refresh, async (sender) =>
            {
                IsCurrentlyRefreshing = true;
                if (CurrentPiData.piHoleData.Url == String.Empty)
                {
                    CurrentPiData.piHoleData = await PiholeDataSerializer.DeserializeData();
                    if (CurrentPiData.piHoleData.Token == "demo") 
                    {
                        CurrentPiData.DemoMode = true;
                    }
                }
                string result;
                if (!CurrentPiData.DemoMode)
                {
                    result = await PiholeHttp.CheckPiholeStatus(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token);
                }
                else 
                {
                    result = "enabled";
                    CurrentPiData.DemoMode = true;
                }
                MessagingCenter.Send(this, Commands.statusUpdate, result);
                IsCurrentlyRefreshing = false;
            });
        }

        private async void Refresh(object obj) 
        {
            IsCurrentlyRefreshing = true;
            string status = await PiholeHttp.CheckPiholeStatus(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token);
            MessagingCenter.Send(this, Commands.statusUpdate, status);
            IsCurrentlyRefreshing = false;
        }

        private async void OnButtonClicked(object timeString) 
        {
            bool successfulCommand = false;
            PiholeHttp.Command command = PiholeHttp.Command.Invalid;
            try
            {
                int time = int.Parse(timeString.ToString());
                if (time < 0)
                {
                    command = PiholeHttp.Command.Enable;
                }
                else 
                {
                    command = PiholeHttp.Command.Disable;
                }
                if (!CurrentPiData.DemoMode)
                {
                    successfulCommand = await PiholeHttp.PiholeCommand(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token,
                        command.ToString().ToLower(), time);
                }
                else 
                {
                    successfulCommand = true;
                }
                if (successfulCommand) 
                {
                    MessagingCenter.Send(this, Commands.statusUpdate, command.ToString().ToLower() + "d");
                }

            }
            catch (Exception err) 
            {
                Console.WriteLine(err + ": " + err.Message);
            }
            if (!successfulCommand) 
            {
                MessagingCenter.Send(this, Commands.statusUpdate, "disconnected");
            }
        }

    }
}