using PiHoleDisablerMultiplatform.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace PiHoleDisablerMultiplatform.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            this.BindingContext = new AboutViewModel();
        }


        private void clearButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}