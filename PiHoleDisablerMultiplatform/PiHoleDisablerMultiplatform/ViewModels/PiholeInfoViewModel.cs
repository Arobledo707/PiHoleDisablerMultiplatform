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
        private const string kDemoModeText = "demo";
        private const string piHoleEntryName = "piholeAddress";

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
        public Command HelpCommand { get; }
        public Command SaveButtonCommand { get; }
        public Command ClearButtonCommand { get; }
        public PiholeInfoViewModel() 
        {
            ScanCommand = new Command(Scanner);
            HelpCommand = new Command(ShowHelpPage);
            SaveButtonCommand = new Command(SaveButonClicked);
            ClearButtonCommand = new Command(ClearPiData);
            Title = "Pi-hole Disabler Info";

            MessagingCenter.Subscribe<PiholeInfoPage>(this, Constants.infoRequest, async (sender) =>
            {
                bool dataSent = await SendPiholeData();
                if (dataSent) 
                {
                    Console.WriteLine("Data sent successfully");
                }
            });

            //MessagingCenter.Subscribe<PiholeInfoPage>(this, Constants.clear, async (sender) =>
            //{
            //    ClearPiData();
            //});

        }

        private async void SaveButonClicked(object obj) 
        {
            ContentPage page = obj as ContentPage;
            if (page == null) 
            {
                return;
            }
            Entry piholeAddress = page.FindByName(piHoleEntryName) as Entry;
            Entry tokenEntered = page.FindByName("tokenEntered") as Entry;
            if (tokenEntered.Text != null && tokenEntered.Text != String.Empty)
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

                if (piholeAddress.Text == null || piholeAddress.Text == String.Empty)
                {
                    piholeAddress.Text = piholeAddress.Placeholder.Trim();
                }

                await ValidateInfo(piholeAddress.Text, tokenEntered.Text.Trim());
            }
            else
            {
                await page.DisplayAlert("Missing Token", "Enter a Token", "Ok");
            }
        }

        private async void ShowHelpPage(Object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(HelpPage)}");
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
                // TODO send message
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
                CurrentPiData.piHoleData = await Serializer.DeserializePiData();
            }
            List<string> data = new List<string> {CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token};
            MessagingCenter.Send(this, Constants.requestedData, data);

            return await Task.FromResult(true);
        }

        private async void ClearPiData(object obj) 
        {
            ContentPage page = obj as ContentPage;
            if (page == null) 
            {
                return;
            }
            Label savedPiholeAddress = page.FindByName<Label>("savedPiholeAddress");
            Label savedToken = page.FindByName<Label>("savedToken");

            if (savedPiholeAddress == null || savedToken == null) 
            {
                return;
            }
            bool clearInfo = await page.DisplayAlert("Clear Pi-hole Info", "Are you sure?", "Yes", "No");

            if (clearInfo)
            {
                if (savedPiholeAddress.Text != null)
                {
                    savedPiholeAddress.Text = String.Empty;
                }
                savedToken.Text = String.Empty;
            }

            bool cleared = await Serializer.DeleteData(Constants.kPiDataFile);
            if (cleared)
            {
                CurrentPiData.piHoleData.Url = String.Empty;
                CurrentPiData.piHoleData.Token = String.Empty;
                CurrentPiData.DemoMode = false;
            }
            else
            {
                MessagingCenter.Send(this, Constants.error, new List<string> { "IO Error", "Failed to delete " + Constants.kPiDataFile});
            }
        }

        private async Task<bool> ValidateInfo(string address, string token) 
        {
            bool isValidated;
            if (token == kDemoModeText)
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
                isSerialized = await Serializer.SerializeDataAsync(CurrentPiData.piHoleData ,Constants.kPiDataFile);
            }
            if (!isSerialized && isValidated) 
            {
                MessagingCenter.Send(this, Constants.error, new List<string> { "IO Error", "Failed to serialize data"});
            }

            return await Task.FromResult(true);

        }
    }
}
