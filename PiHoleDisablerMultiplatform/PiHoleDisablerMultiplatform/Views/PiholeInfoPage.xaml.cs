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
        //private Label savedAddress;

        private Entry enteredToken;
        // private Label savedToken;

        Action<PiholeInfoViewModel> testAction;

        PiholeInfoViewModel piViewModel;
        string message;
        public PiholeInfoPage()
        {
            InitializeComponent();
            this.BindingContext = new PiholeInfoViewModel();

            enteredAddress = FindByName("piholeAddress") as Entry;
            enteredToken = FindByName("tokenEntered") as Entry;
            piViewModel = this.BindingContext as PiholeInfoViewModel;
            MessagingCenter.Subscribe(piViewModel, "testmessage", (object sender, string arg) =>
            {
                string test = arg;
            });
            //MessagingCenter.Subscribe<PiholeInfoViewModel>(piViewModel, "testmessage", test);
            //testAction += OnRecieve();


        }

        private void OnRecieve() 
        {
            testAction.Invoke(piViewModel);
           // testAction
            //MessagingCenter.Subscribe<PiholeInfoViewModel>(this, "testmessage", testAction);
           // MessagingCenter.Subscribe<object, string>(this, "message", (this.BindingContext, "lol", "dumb"));
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
            if (enteredAddress.Text == null)
            {
                savedPiholeAddress.Text = enteredAddress.Placeholder.Trim();
            }
            else 
            {
                savedPiholeAddress.Text = enteredAddress.Text.Trim();
                enteredAddress.Text = String.Empty;
            }
            if (savedPiholeAddress.Text != null)
            {
                piViewModel.savedAddress = savedPiholeAddress.Text;
            }

            if (enteredToken.Text != null) 
            {
                savedToken.Text = enteredToken.Text.Trim();
                enteredToken.Text = String.Empty;
                piViewModel.savedToken = savedToken.Text;
            }
        }

    }
}