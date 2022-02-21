using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class DisableViewModel : BaseViewModel
    {
        public Command DisableCommand { get; }
        public Command EnableCommand { get; }
        public DisableViewModel() 
        {
            Title = "Disable";
            DisableCommand = new Command(OnDisableButtonClicked);
            EnableCommand = new Command(OnEnableButtonClicked);
        }

        private async void OnDisableButtonClicked(object obj) 
        {

        }

        private async void OnEnableButtonClicked() 
        {

        }
    }
}