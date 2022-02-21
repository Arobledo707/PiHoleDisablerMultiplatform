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
            SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
            //Shell.Current.FindByName<Entry>();
        }

        private async void OnClearButtonClicked(object obj) 
        {

        }
        private async void OnSaveButtonClicked(object obj) 
        {

        }
    }
}
