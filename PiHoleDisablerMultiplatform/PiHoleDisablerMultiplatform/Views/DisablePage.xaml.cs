using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisablePage : ContentPage
    {
        private const string kPiHoleStatusStartString = "Pi-hole status: ";
        private const string KEnabled = "enabled";
        private const string kDisconnected = "disconnected";
        private const string kChooseTheme = "Choose Theme";

        public DisablePage()
        {
            InitializeComponent();
            this.BindingContext = new DisableViewModel();
            MessagingCenter.Subscribe<DisableViewModel, string>(this, Constants.statusUpdate, async (sender, status) =>
            {
                ChangeStatus(status);
            });

        }

        protected void ChangeStatus(string status) 
        {
            piHoleStatusText.Text = kPiHoleStatusStartString + status;
            if (status == KEnabled || status == kDisconnected)
            {
                disableGrid.IsVisible = true;
                enableData.IsVisible = false;
            }
            else 
            {
                disableGrid.IsVisible = false;
                enableData.IsVisible = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.refresh);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
           var result = await DisplayActionSheet(kChooseTheme, Constants.cancel, null, 
                Constants.Theme.Default.ToString(), Constants.Theme.Blue.ToString(), Constants.Theme.Green.ToString(), 
                Constants.Theme.Orange.ToString(), Constants.Theme.Purple.ToString());
        }
    }
}