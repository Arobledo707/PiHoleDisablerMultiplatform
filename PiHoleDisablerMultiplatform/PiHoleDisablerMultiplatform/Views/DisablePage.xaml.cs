using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisablePage : ContentPage
    {
        private readonly string piHoleStatusStartString = "Pi-hole status: ";
        private readonly string enabled = "enabled";
        private readonly string disconnected = "disconnected";

        private DisableViewModel viewModel;

        public DisablePage()
        {
            InitializeComponent();
            viewModel = new DisableViewModel();
            BindingContext = viewModel;
            MessagingCenter.Subscribe<DisableViewModel, string>(this, Commands.statusUpdate, async (sender, status) =>
            {
                ChangeStatus(status);
            });

        }

        protected void ChangeStatus(string status) 
        {
            piHoleStatusText.Text = piHoleStatusStartString + status;
            if (status == enabled || status == disconnected)
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
            MessagingCenter.Send(this, Commands.refresh);
        }


    }
}