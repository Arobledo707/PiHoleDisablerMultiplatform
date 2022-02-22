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
        public PiholeInfoPage()
        {
            InitializeComponent();
            this.BindingContext = new PiholeInfoViewModel();
            enteredAddress = FindByName("piholeAddress") as Entry;
        }
    }
}