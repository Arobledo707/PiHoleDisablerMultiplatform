﻿using System;
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
		public QueryPage()
		{
			InitializeComponent();
			this.BindingContext = new QueryViewModel();
		}
	}
}