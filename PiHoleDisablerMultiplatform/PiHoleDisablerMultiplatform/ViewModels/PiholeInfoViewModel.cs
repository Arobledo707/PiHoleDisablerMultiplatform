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

        private string validInfo = "validInfo";
        private string checkInfo = "checkInfo";
        private string requestInfo = "requestSerializedInfo";

        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";

            SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, checkInfo, async (sender, arg) => 
            {
                ValidateInfo(arg[0], arg[1]);
                //string result = await PiholeHttp.CheckPiholeStatus(arg[0], arg[1]);
                //MessagingCenter.Send(this, validInfo, result != "disconnected");
            });

            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, requestInfo, async (sender, arg) =>
            {
                if (CurrentPiData.piHoleData == null) 
                {
                    CurrentPiData.piHoleData = await PiholeDataSerializer.DeserializeData();
                }

            });
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
        private async void OnSaveButtonClicked() 
        {
        }

        private async void ValidateInfo(string address, string token) 
        {
            bool isValidated = await PiholeHttp.PiholeCommand(address, token, "enable", 0);
            if (isValidated) 
            {
                PiHoleData pData = new PiHoleData(address, token);
                isValidated = await PiholeDataSerializer.SerializeData(pData);
            }
            MessagingCenter.Send(this, validInfo, isValidated);
        }
    }
}
