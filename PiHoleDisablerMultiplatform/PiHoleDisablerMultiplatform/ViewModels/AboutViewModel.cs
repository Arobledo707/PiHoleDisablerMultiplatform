using PiHoleDisablerMultiplatform.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public Command SaveInfoCommand { get; }
        public AboutViewModel()
        {
            Title = "Pi-Hole Disabler";
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            SaveInfoCommand = new Command(OnSaveButtonClicked);
        }
        private async void OnSaveButtonClicked(object obj) 
        {
            //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            //await Shell.Current.GoToAsync($"//{nameof(DisablePage)}");
        }
        //public ICommand OpenWebCommand { get; }
    }
}