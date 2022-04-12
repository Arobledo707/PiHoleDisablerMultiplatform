using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.Models;
using PiHoleDisablerMultiplatform.StaticPi;
using System.Collections.Generic;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class PiholeInfoViewModel : BaseViewModel
    {
        private const string kEnterInfoText = "Enter Info";
        private const string kPiHoleAddressText = "Pi-hole Address: ";
        private const string kAddressPlaceholder = "pi.hole";
        private const string kQrButtonText = "QR";
        private const string kTokenText = "Token: ";
        private const string kTokenPlacerholderText = "Enter Token Here";
        private const string kClearButtonText = "Clear";
        private const string kSavedInfoText = "Saved Info";
        private const string kSaveButtonText = "Save";
        private const string kHelpButtonText = "Help";

        public string EnterInfoText { get { return kEnterInfoText; } }
        public string PiholeAddressText { get { return kPiHoleAddressText; } }
        public string AddressPlaceholder { get { return kAddressPlaceholder; } }
        public string TokenText { get { return kTokenText; } }
        public string TokenPlaceholderText { get { return kTokenPlacerholderText; } }
        public string QrButtonText { get { return kQrButtonText; } }
        public string ClearButtonText { get { return kClearButtonText; } }
        public string SavedInfoText { get { return kSavedInfoText; } }
        public string SaveButtonText { get { return kSaveButtonText; } }
        public string HelpButtonText { get { return kHelpButtonText; } }



        public Command ScanCommand { get; }
        public PiholeInfoViewModel() 
        {
            ScanCommand = new Command(Scanner);

            Title = "Pi-hole Disabler Info";
            
            MessagingCenter.Subscribe<PiholeInfoPage, List<string>>(this, Constants.checkInfo, async (sender, arg) => 
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

            MessagingCenter.Subscribe<PiholeInfoPage>(this, Constants.infoRequest, async (sender) =>
            {
                bool dataSent = await SendPiholeData();
                if (dataSent) 
                {
                    Console.WriteLine("Data sent successfully");
                }
            });

            MessagingCenter.Subscribe<PiholeInfoPage>(this, Constants.clear, async (sender) =>
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
            MessagingCenter.Send(this, Constants.requestedData, data);

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
                MessagingCenter.Send(this, Constants.error, new List<string> { "IO Error", "Failed to delete file"});
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
            MessagingCenter.Send(this, Constants.validInfo, isValidated);
            if (isValidated) 
            {
                CurrentPiData.piHoleData = new PiHoleData(address, token);
                isSerialized = await PiholeDataSerializer.SerializeData(CurrentPiData.piHoleData);
            }
            if (!isSerialized && isValidated) 
            {
                MessagingCenter.Send(this, Constants.error, new List<string> { "IO Error", "Failed to serialize data"});
            }

            return await Task.FromResult(true);

        }
    }
}
