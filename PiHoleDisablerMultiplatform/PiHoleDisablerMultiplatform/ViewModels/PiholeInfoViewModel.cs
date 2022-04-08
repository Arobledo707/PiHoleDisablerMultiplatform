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
    public class PiholeInfoViewModel : BaseViewModel
    {
        private readonly string enterInfoText = "Enter Info";
        private readonly string piHoleAddressText = "Pi-hole Address: ";
        private readonly string addressPlaceholder = "pi.hole";
        private readonly string qrButtonText = "QR";
        private readonly string tokenText = "Token: ";
        private readonly string tokenPlacerholderText = "Enter Token Here";
        private readonly string clearButtonText = "Clear";
        private readonly string savedInfoText = "Saved Info";
        private readonly string saveButtonText = "Save";
        private readonly string helpButtonText = "Help";

        public string EnterInfoText { get { return enterInfoText; } }
        public string PiholeAddressText { get { return piHoleAddressText; } }
        public string AddressPlaceholder { get { return addressPlaceholder; } }
        public string TokenText { get { return tokenText; } }
        public string TokenPlaceholderText { get { return tokenPlacerholderText; } }
        public string QrButtonText { get { return qrButtonText; } }
        public string ClearButtonText { get { return clearButtonText; } }
        public string SavedInfoText { get { return savedInfoText; } }
        public string SaveButtonText { get { return saveButtonText; } }
        public string HelpButtonText { get { return helpButtonText; } }



        public Command ScanCommand { get; }
        public PiholeInfoViewModel() 
        {
            ScanCommand = new Command(Scanner);

            Title = "Pi-hole Disabler Info";
            
            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, Commands.checkInfo, async (sender, arg) => 
            {
                var permissionStatus = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
                if (permissionStatus != PermissionStatus.Granted)
                {
                    permissionStatus = await Permissions.RequestAsync<Permissions.NetworkState>();
                }
                if (permissionStatus != PermissionStatus.Granted)
                {
                    //todo send message about network state is needed
                    return;
                }
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


        private async void Scanner(Object obj) 
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted) 
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
            }
            if (permissionStatus != PermissionStatus.Granted) 
            {
                //
                return;
            }
            try 
            {
                var scanner = DependencyService.Get<IQRScan>();
                string result = await scanner.AsyncScan();
                if (result != null) 
                {
                    Entry entry = obj as Entry;
                    entry.Text = result;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err + ": " + err.Message);
            }
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

        private async void ClearData() 
        {
            bool cleared = await PiholeDataSerializer.DeleteData();
            if (cleared)
            {
                CurrentPiData.piHoleData.Url = String.Empty;
                CurrentPiData.piHoleData.Token = String.Empty;
                CurrentPiData.DemoMode = false;
            }
            else
            {
                MessagingCenter.Send(this, Commands.error, new List<string> { "IO Error", "Failed to delete file"});
            }
        }

        private async Task<bool> ValidateInfo(string address, string token) 
        {
            bool isValidated;
            if (token == "demo")
            {
                StaticPi.CurrentPiData.DemoMode = true;
                isValidated = true;
            }
            else 
            {
                CurrentPiData.DemoMode = false;
                isValidated = await PiholeHttp.PiholeCommand(address, token, "enable", 0);
            }
            bool isSerialized = false;
            MessagingCenter.Send(this, Commands.validInfo, isValidated);
            if (isValidated) 
            {
                CurrentPiData.piHoleData = new PiHoleData(address, token);
                isSerialized = await PiholeDataSerializer.SerializeData(CurrentPiData.piHoleData);
            }
            if (!isSerialized && isValidated) 
            {
                MessagingCenter.Send(this, Commands.error, new List<string> { "IO Error", "Failed to serialize data"});
            }

            return await Task.FromResult(true);

        }
    }
}
