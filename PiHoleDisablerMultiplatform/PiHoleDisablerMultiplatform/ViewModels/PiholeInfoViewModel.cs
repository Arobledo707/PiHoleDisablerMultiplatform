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

        private readonly string validInfo = "validInfo";
        private readonly string checkInfo = "checkInfo";
        private readonly string infoRequest = "requestInfo";
        private readonly string requestedData = "requestedData";

        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";
            

            //SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, checkInfo, async (sender, arg) => 
            {
                await ValidateInfo(arg[0], arg[1]);
            });

            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, infoRequest, async (sender, arg) =>
            {
                await SendPiholeData();
            });
        }

        private async Task<bool> SendPiholeData()
        {
            if (CurrentPiData.piHoleData.Url == String.Empty)
            {
                CurrentPiData.piHoleData = await PiholeDataSerializer.DeserializeData();
            }
            List<string> data = new List<string> {CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token};
            MessagingCenter.Send(this, requestedData, data);

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

        private async Task<bool> ValidateInfo(string address, string token) 
        {
            bool isValidated = await PiholeHttp.PiholeCommand(address, token, "enable", 0);
            if (isValidated) 
            {
                CurrentPiData.piHoleData = new PiHoleData(address, token);
                isValidated = await PiholeDataSerializer.SerializeData(CurrentPiData.piHoleData);
            }
            MessagingCenter.Send(this, validInfo, isValidated);

            return await Task.FromResult(true);

        }
    }
}
