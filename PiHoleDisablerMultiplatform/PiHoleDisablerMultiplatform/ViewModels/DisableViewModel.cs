using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;


namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class DisableViewModel : BaseViewModel
    {
        private readonly string statusText = "Pi-hole status: ";
        private readonly string disable30seconds = "Disable For 30 Seconds";
        private readonly string disable60seconds = "Disable For 60 Seconds";
        private readonly string disable5minutes = "Disable For 5 Minutes";
        private readonly string disable30minutes = "Disable For 30 Minutes";


        public string StatusText { get { return statusText; } }

        public string Disable30SecondsText { get { return disable30seconds; } }
        public string Disable60SecondsText { get { return disable60seconds; } }
        public string Disable5MinutesText { get { return disable5minutes; } }
        public string Disable30MinutesText { get { return disable30minutes; } }

        public Command ButtonClickCommand { get; }
        public Command RefreshCommand { get; }

        public Command ThemeCommand { get; }

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
            ThemeCommand = new Command(ToolBarClicked);

            MessagingCenter.Subscribe<DisablePage>(this, Constants.refresh, async (sender) =>
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
                MessagingCenter.Send(this, Constants.statusUpdate, result);
                IsCurrentlyRefreshing = false;
            });
        }

        private async void Refresh(object obj) 
        {
            IsCurrentlyRefreshing = true;
            string status = await PiholeHttp.CheckPiholeStatus(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token);
            MessagingCenter.Send(this, Constants.statusUpdate, status);
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
                    MessagingCenter.Send(this, Constants.statusUpdate, command.ToString().ToLower() + "d");
                }

            }
            catch (Exception err) 
            {
                Console.WriteLine(err + ": " + err.Message);
            }
            if (!successfulCommand) 
            {
                MessagingCenter.Send(this, Constants.statusUpdate, "disconnected");
            }
        }

        private async void ToolBarClicked(object obj) 
        {
            Page page = obj as Page;
            var result = await page.DisplayActionSheet("Choose Theme", "cancel", null,
                Constants.Theme.Default.ToString(), Constants.Theme.Blue.ToString(), 
                Constants.Theme.Green.ToString(), Constants.Theme.Orange.ToString(), 
                Constants.Theme.Purple.ToString(), Constants.Theme.Grey.ToString());

            if (result != null) 
            {
                DetectThemeChoice(result);
            }
        }

        private void DetectThemeChoice(string themeString) 
        {
            if (themeString == Constants.cancel)
            {
                return;
            }
            else if (themeString == Constants.Theme.Default.ToString())
            {
                CurrentPiData.currentTheme = Constants.Theme.Default;
            }
            else if (themeString == Constants.Theme.Blue.ToString())
            {
                CurrentPiData.currentTheme = Constants.Theme.Blue;
            }
            else if (themeString == Constants.Theme.Green.ToString())
            {
                CurrentPiData.currentTheme = Constants.Theme.Green;
            }
            else if (themeString == Constants.Theme.Orange.ToString())
            {
                CurrentPiData.currentTheme = Constants.Theme.Orange;
            }
            else if (themeString == Constants.Theme.Purple.ToString())
            {
                CurrentPiData.currentTheme = Constants.Theme.Purple;
            }
            else if (themeString == Constants.Theme.Grey.ToString()) 
            {
                CurrentPiData.currentTheme = Constants.Theme.Grey;
            }
            App app = Application.Current as App;
            app.SetThemes();
            app.ChangeTheme();
        }

    }
}