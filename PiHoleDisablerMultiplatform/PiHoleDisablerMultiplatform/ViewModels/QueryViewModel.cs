using System;
using System.Collections.Generic;
using System.Text;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class QueryViewModel : BaseViewModel
    {
        private bool isRefreshing = false;
        public bool IsCurrentlyRefreshing 
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        public Command RefreshCommand { get;}

        public QueryViewModel()
        {
            RefreshCommand = new Command(Refresh);
            Title = "Queries";
            Test();
        }

        private async void Test() 
        {
            string contentString = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, 10);
            if (contentString == String.Empty || contentString == null) 
            {
                return;
            }
            QueryData data = JsonConvert.DeserializeObject<QueryData>(contentString);

        }

        private async void Refresh() 
        {
            //IsCurrentlyRefreshing = true;
            StackLayout stackLayout = CreateStackLayout(Color.LightGreen);

            string contentString = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, 10);
            if (contentString == String.Empty || contentString == null)
            {
                return;
            }
            QueryData data = JsonConvert.DeserializeObject<QueryData>(contentString);
            IsCurrentlyRefreshing = false;
        }

        private StackLayout CreateStackLayout(Color color) 
        {
            StackLayout stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            stackLayout.FlowDirection = FlowDirection.LeftToRight;
            stackLayout.BackgroundColor = color;

            return stackLayout;
        }

        private void AttachItems(ref StackLayout layout) 
        {
            Label label = new Label();
            label.Text = "lol";
            //layout.Children.Add();
        }
    }
}
