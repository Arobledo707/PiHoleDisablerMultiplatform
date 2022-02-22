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

        private Button saveButton;
        private Button clearButton;
        private Entry tokenEntered;
        private Label tokenSaved;

        private Entry tempAddress;
        private Entry savedAddress;

        private readonly string saveButtonName = "saveButton";
        private readonly string clearButtonName = "clearButton";
        public PiholeInfoViewModel() 
        {
            Title = "Pi-hole Disabler Info";
            SaveInfoCommand = new Command(OnSaveButtonClicked);
            ClearInfoCommand = new Command(OnClearButtonClicked);
        }

        private async void OnClearButtonClicked() 
        {
            if (clearButton is null) 
            {
                saveButton = App.Current.FindByName<Button>(clearButtonName);
            }
        }
        private async void OnSaveButtonClicked() 
        {
            if (saveButton is null)
            {
                saveButton = App.Current.FindByName<Button>(saveButtonName);
            }

            if (tempAddress is null) 
            {
                tempAddress = App.Current.FindByName<Entry>("piholeAddress");
            }

        }
    }
}
