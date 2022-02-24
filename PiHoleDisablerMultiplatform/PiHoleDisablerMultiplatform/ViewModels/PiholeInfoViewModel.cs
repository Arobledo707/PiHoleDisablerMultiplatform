using PiHoleDisablerMultiplatform.Services;
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

        public string savedAddress { get; set; }
        public string savedToken { get; set; }

        public bool infoCleared { get; set; }
        public bool infoSaved { get; set; }

        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";
            SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
        }


        private async void OnClearButtonClicked(Object obj) 
        {
            if (infoCleared) 
            {
                bool cleared = await PiholeDataSerializer.DeleteData();
                if (cleared) 
                {
                }
            }
        }
        private async void OnSaveButtonClicked() 
        {
            MessagingCenter.Send(this, "testmessage", "lololololol");

        }
    }
}
