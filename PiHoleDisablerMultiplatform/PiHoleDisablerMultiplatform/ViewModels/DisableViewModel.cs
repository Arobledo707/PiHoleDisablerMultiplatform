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
        public DisableViewModel() 
        {
            Title = "Disable";
            ButtonClickCommand = new Command(OnButtonClicked);

            MessagingCenter.Subscribe<DisablePage>(this, Commands.refresh, async (sender) =>
            {
                string result = await PiholeHttp.CheckPiholeStatus(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token);
                MessagingCenter.Send(this, Commands.statusUpdate, result);
            });
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
                successfulCommand = await PiholeHttp.PiholeCommand(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token,
                    command.ToString().ToLower(), time);
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