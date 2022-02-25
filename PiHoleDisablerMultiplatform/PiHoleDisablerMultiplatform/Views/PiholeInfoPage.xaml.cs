using PiHoleDisablerMultiplatform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiHoleDisablerMultiplatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PiholeInfoPage : ContentPage
    {
        private Entry enteredAddress;

        private Entry enteredToken;
        PiholeInfoViewModel piViewModel;
        private readonly string checkInfo = "checkInfo";
        private readonly string validInfo = "validInfo";

        public PiholeInfoPage()
        {
            InitializeComponent();
            this.BindingContext = new PiholeInfoViewModel();

            enteredAddress = FindByName("piholeAddress") as Entry;
            enteredToken = FindByName("tokenEntered") as Entry;
            piViewModel = this.BindingContext as PiholeInfoViewModel;

            MessagingCenter.Subscribe<PiholeInfoViewModel, bool>(this, validInfo, async (sender, arg) =>
            {
                if (arg)
                {
                    MoveData();
                }
                else 
                {
                    await DisplayAlert("Pi-hole unreachable", "Info is either incorrect or Pi-hole is unreachable", "Ok");
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedsPiholeData()) 
            {
                MessagingCenter.Send(this, "requestSerializedInfo");
            }
        }

        private bool NeedsPiholeData() 
        {
            return (savedPiholeAddress == null || savedToken == null);
        }

        private void MoveData() 
        {
            if (enteredAddress.Text == null)
            {
                savedPiholeAddress.Text = enteredAddress.Placeholder.Trim();
            }
            else
            {
                savedPiholeAddress.Text = enteredAddress.Text.Trim();
                enteredAddress.Text = String.Empty;
            }

            if (enteredToken.Text != null)
            {
                savedToken.Text = enteredToken.Text.Trim();
                enteredToken.Text = String.Empty;
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
                piViewModel.savedToken = String.Empty;
                piViewModel.savedAddress = String.Empty;
            }
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (enteredToken.Text != null)
            {
                string sendAddress;
                if (enteredAddress.Text == null)
                {
                    sendAddress = enteredAddress.Placeholder.Trim();
                }
                else 
                {
                    sendAddress = enteredAddress.Text.Trim();   
                }

                List<String> checkStrings = new List<string> { sendAddress, enteredToken.Text.Trim() };

                MessagingCenter.Send(this, checkInfo, checkStrings);
            }
        }

    }
}