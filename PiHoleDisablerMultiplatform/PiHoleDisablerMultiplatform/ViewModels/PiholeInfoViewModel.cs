using PiHoleDisablerMultiplatform.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    //INotifyPropertyChanged
    public class PiholeInfoViewModel : BaseViewModel
    {
        public Command SaveInfoCommand { get; }
        public Command ClearInfoCommand { get; }

        public ObservableCollection<string> info;

        public string savedAddress { get; set; }
        public string savedToken { get; set; }

        //private string savedAddress;

        private readonly string saveButtonName = "saveButton";
        private readonly string clearButtonName = "clearButton";
        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";
            info = new ObservableCollection<string>();
            SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
        }


        private async void OnClearButtonClicked(Object obj) 
        {
            obj.ToString();
        }
        private async void OnSaveButtonClicked() 
        {

        }
    }
}
