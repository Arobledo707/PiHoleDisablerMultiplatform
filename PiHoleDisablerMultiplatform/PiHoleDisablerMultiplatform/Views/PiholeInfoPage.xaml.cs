using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PiholeInfoPage : ContentPage
    {
        private readonly string piHoleDisablerString = "PiHoleDisabler";
        PiholeInfoViewModel piViewModel;

        public PiholeInfoPage()
        {
            InitializeComponent();
            this.BindingContext = new PiholeInfoViewModel();
            piViewModel = this.BindingContext as PiholeInfoViewModel;

            MessagingCenter.Subscribe<PiholeInfoViewModel, bool>(this, Commands.validInfo, async (sender, arg) =>
            {
                if (arg)
                {
                    MoveData();
                    if (App.Current.MainPage.Title != piHoleDisablerString)
                    {
                        App.Current.MainPage = new AppShell();
                    }
                }
                else
                {
                    await DisplayAlert("Pi-hole unreachable", "Info is either incorrect or Pi-hole is unreachable", "Ok");
                }
            });

            MessagingCenter.Subscribe<PiholeInfoViewModel, List<string>>(this, Commands.requestedData, async (sender, infoStrings) =>
            {
                savedPiholeAddress.Text = infoStrings[0];
                savedToken.Text = infoStrings[1];
            });

            MessagingCenter.Subscribe<PiholeInfoViewModel, List<string>>(this, Commands.error, async (sender, message) =>
            {
                await DisplayAlert(message[0], message[1], "Ok");
            });
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedsPiholeData())
            {
                MessagingCenter.Send(this, Commands.infoRequest);
            }
        }

        private bool NeedsPiholeData()
        {
            return ((savedPiholeAddress.Text == null || savedPiholeAddress.Text == String.Empty) ||
                 (savedToken.Text == null || savedToken.Text == String.Empty));
        }

        private void MoveData()
        {
            if (piholeAddress.Text == null)
            {
                savedPiholeAddress.Text = piholeAddress.Placeholder.Trim();
            }
            else
            {
                savedPiholeAddress.Text = piholeAddress.Text.Trim();
                piholeAddress.Text = String.Empty;
            }

            if (tokenEntered.Text != null)
            {
                savedToken.Text = tokenEntered.Text.Trim();
                tokenEntered.Text = String.Empty;
            }
        }



        private async void clearButton_Clicked(object sender, EventArgs e)
        {
            bool clearInfo = await DisplayAlert("Clear Pi-hole Info", "Are you sure?", "Yes", "No");
            if (clearInfo)
            {
                if (savedPiholeAddress.Text != null)
                {
                    savedPiholeAddress.Text = String.Empty;
                }
                savedToken.Text = String.Empty;
                MessagingCenter.Send(this, Commands.clear);
            }
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (tokenEntered.Text != null && tokenEntered.Text != String.Empty)
            {
                string sendAddress;
                if (piholeAddress.Text == null || piholeAddress.Text == String.Empty)
                {
                    sendAddress = piholeAddress.Placeholder.Trim();
                }
                else
                {
                    sendAddress = piholeAddress.Text.Trim();
                }

                List<String> checkStrings = new List<string> { sendAddress, tokenEntered.Text.Trim() };

                MessagingCenter.Send(this, Commands.checkInfo, checkStrings);
            }
            else
            {
                await DisplayAlert("Missing Token", "Enter a Token", "Ok");
            }
        }

    }
}