using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PiHoleDisablerMultiplatform.ViewModels;

namespace PiHoleDisablerMultiplatform.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QueryPage : ContentPage
	{
		private QueryViewModel viewModel;
		public QueryPage()
		{
			InitializeComponent();
			viewModel = new QueryViewModel();
			this.BindingContext = viewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			viewModel.RefreshCommand.Execute(scrollView);
        }


    }
}