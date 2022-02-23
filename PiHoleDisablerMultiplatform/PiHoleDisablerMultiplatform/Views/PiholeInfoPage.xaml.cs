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
        private Label savedAddress;

        private Entry enteredToken;
        private Label savedToken;

        PiholeInfoViewModel piViewModel;
        public PiholeInfoPage()
        {
            InitializeComponent();
            this.BindingContext = new PiholeInfoViewModel();

            enteredAddress = FindByName("piholeAddress") as Entry;
            enteredToken = FindByName("tokenEntered") as Entry;
            savedAddress = FindByName("savedPiholeAddress") as Label;
            savedToken = FindByName("savedtoken") as Label;
            piViewModel = this.BindingContext as PiholeInfoViewModel;
        }

        private void clearButton_Clicked(object sender, EventArgs e)
        {

        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (enteredAddress.Text.Length == 0)
            {
                savedAddress.Text = enteredAddress.Placeholder.Trim();
            }
            else 
            {
                savedAddress.Text = enteredAddress.Text.Trim();
                enteredAddress.Text = String.Empty;
            }
            if (savedAddress.Text.Length > 0)
            {
                piViewModel.savedAddress = savedAddress.Text;
            }

            if (enteredToken.Text.Length > 0) 
            {
                savedToken.Text = enteredToken.Text.Trim();
                enteredToken.Text = String.Empty;
                piViewModel.savedToken = savedToken.Text;
            }
        }
    }
}