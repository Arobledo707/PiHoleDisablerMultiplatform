using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.Models;
using PiHoleDisablerMultiplatform.StaticPi;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    //INotifyPropertyChanged
    public class PiholeInfoViewModel : BaseViewModel
    {
        public Command SaveInfoCommand { get; }
        public Command ClearInfoCommand { get; }

        public string savedAddress { get; set; }
        public string savedToken { get; set; }

        public bool infoCleared { get; set; }
        public bool infoSaved { get; set; }


        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";
            

            //SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, Commands.checkInfo, async (sender, arg) => 
            {
                await ValidateInfo(arg[0], arg[1]);
            });

            MessagingCenter.Subscribe<PiholeInfoPage>(this, Commands.infoRequest, async (sender) =>
            {
                bool dataSent = await SendPiholeData();
                if (dataSent) 
                {
                    Console.WriteLine("Data sent successfully");
                }
            });

            MessagingCenter.Subscribe<PiholeInfoPage>(this, Commands.clear, async (sender) =>
            {
                ClearData();
            });

        }

        private async Task<bool> SendPiholeData()
        {
            if (CurrentPiData.piHoleData.Url == String.Empty)
            {
                CurrentPiData.piHoleData = await PiholeDataSerializer.DeserializeData();
            }
            List<string> data = new List<string> {CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token};
            MessagingCenter.Send(this, Commands.requestedData, data);

            return await Task.FromResult(true);
        }

        private async void OnClearButtonClicked(Object obj) 
        {
            if (infoCleared) 
            {
                bool cleared = await PiholeDataSerializer.DeleteData();
                if (cleared) 
                {
                    CurrentPiData.piHoleData = null;
                }
            }
        }

        private async void ClearData() 
        {
            bool cleared = await PiholeDataSerializer.DeleteData();
            if (cleared)
            {
                CurrentPiData.piHoleData.Url = String.Empty;
                CurrentPiData.piHoleData.Token = String.Empty;
            }
            else
            {
                MessagingCenter.Send(this, Commands.error, new List<string> { "IO Error", "Failed to delete file"});
            }
        }

        private async Task<bool> ValidateInfo(string address, string token) 
        {
            bool isValidated = await PiholeHttp.PiholeCommand(address, token, "enable", 0);
            MessagingCenter.Send(this, Commands.validInfo, isValidated);
            if (isValidated) 
            {
                CurrentPiData.piHoleData = new PiHoleData(address, token);
                isValidated = await PiholeDataSerializer.SerializeData(CurrentPiData.piHoleData);
            }
            if (!isValidated) 
            {
                MessagingCenter.Send(this, Commands.error, new List<string> { "IO Error", "Failed to serialize data"});
            }

            return await Task.FromResult(true);

        }
    }
}
