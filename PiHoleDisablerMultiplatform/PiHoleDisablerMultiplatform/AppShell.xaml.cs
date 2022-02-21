using PiHoleDisablerMultiplatform.ViewModels;
using PiHoleDisablerMultiplatform.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            //Routing.RegisterRoute(nameof(DisablePage), typeof(DisablePage));
        }

    }
}
