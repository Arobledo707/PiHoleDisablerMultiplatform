using System;
using System.Collections.Generic;
using PiHoleDisablerMultiplatform.StaticPi;
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
			MessagingCenter.Subscribe<QueryViewModel, List<string>>(this, Commands.listChange, async(sender, message) => 
			{
				if (message.Count < 2) 
				{
					Console.WriteLine("Error: message has less than 2 strings");
					return;
				}
				await DisplayAlert(message[0], message[1], "Ok");
			});
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			viewModel.RefreshCommand.Execute(scrollView);
		}

    }
}