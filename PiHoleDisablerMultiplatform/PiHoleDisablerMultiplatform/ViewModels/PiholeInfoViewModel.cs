using PiHoleDisablerMultiplatform.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class PiholeInfoViewModel : BaseViewModel
    {
        public Command SaveInfoCommand { get; }
        public Command ClearInfoCommand { get; }
        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";
            SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
        }

        private async void OnClearButtonClicked(object obj) 
        {

        }
        private async void OnSaveButtonClicked(object obj) 
        {

        }
    }
}
