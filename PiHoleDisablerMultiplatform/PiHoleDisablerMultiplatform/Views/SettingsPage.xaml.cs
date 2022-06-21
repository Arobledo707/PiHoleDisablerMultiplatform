using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiHoleDisablerMultiplatform.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiHoleDisablerMultiplatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        SettingsViewModel viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            viewModel = new SettingsViewModel();
            this.BindingContext = viewModel;
        }

        private void hourCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            viewModel.Check24HourCommand.Execute(sender);
        }

        private void timeOnlyCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            viewModel.CheckTimeOnlyCommand.Execute(sender);
        }
    }
}