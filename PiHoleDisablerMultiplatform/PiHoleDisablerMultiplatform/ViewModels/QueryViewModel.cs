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

        private async void Refresh(object param) 
        {

            string contentString = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, 30);
            if (contentString == String.Empty || contentString == null)
            {
                return;
            }
            ContentPage page = param as ContentPage;
            ScrollView scrollView  = page.Content.FindByName<ScrollView>("flexLayout");
            if (scrollView == null) 
            {
                return;
            }
            var test = scrollView.Content;
            StackLayout content = test as StackLayout;
            var labelInfo = content.Children[0];
            content.Children.Clear();
            content.Children.Add(labelInfo);
            //flexLayout.Children.Clear();
            QueryData queryData = JsonConvert.DeserializeObject<QueryData>(contentString);
            //flexLayout.Children.Add(labelInfo);
            var enumerate = Application.Current.Resources.MergedDictionaries.GetEnumerator();
            enumerate.MoveNext();
            ResourceDictionary currentTheme = enumerate.Current;

            foreach (List<string> stringList in queryData.data) 
            {
                Color colour = Color.Red;
                object colored;
                Color buttonColor = Color.White;

                if (stringList[4] != "1")
                {
                    if (currentTheme.TryGetValue("AllowedQueryColor", out colored))
                    {
                        colour = (Color)colored;
                    }
                }
                else 
                {
                    if (currentTheme.TryGetValue("BlockedQueryColor", out colored))
                    {
                        colour = (Color)colored;
                    }

                    if (currentTheme.TryGetValue("WhitelistButtonColor", out colored))
                    {
                        buttonColor = (Color)colored;
                    }

                }
                StackLayout stackLayout = CreateStackLayout(colour);
                for (int i = 0; i < 4;++i) 
                {
                    double widthRequest = 0;
                    double fontSize = 12;
                    string text = stringList[i];
                    switch (i) 
                    {
                        case 0:
                            widthRequest = 60;
                            long epoch = long.Parse(text);
                            DateTimeOffset dtOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(text));
                            DateTime dt = dtOffset.DateTime;
                            text = dtOffset.DateTime.ToString();
                            fontSize = 11;
                            break;
                        case 1:
                            widthRequest = 50;
                            break;
                        case 2:
                            widthRequest = 120;
                            break;
                        case 3:
                            widthRequest = 80;
                            break;
                        case 4:
                            widthRequest = 50;
                            break;
                    }
                    stackLayout.Children.Add(CreateLabel(text, fontSize, widthRequest));
                }

                stackLayout.Children.Add(CreateButton(stringList[4], buttonColor));
                content.Children.Add(stackLayout);
                //flexLayout.Children.Add(stackLayout);
                //var flexLayout.Content 
            }

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

        private Label CreateLabel(string text, double fontSize, double widthRequest) 
        {
            Label label = new Label();
            label.HorizontalOptions = LayoutOptions.FillAndExpand;
            label.WidthRequest = widthRequest;
            label.Padding = new Thickness(0, 0, 10, 0);
            label.FontSize = fontSize;
            label.Text = text;
            return label;
        }

        private void AttachItems(ref StackLayout layout) 
        {
            Label label = new Label();
            label.Text = "lol";
            //layout.Children.Add();
        }

        private Button CreateButton(string status, Color color) 
        {

            Button button = new Button();
            button.FontSize = 11;
            if (color != Color.White)
            {
                button.BackgroundColor = color;
            }
            if (status != "1")
            {
                button.Text = "blacklist";
            }
            else 
            {
                button.Text = "whitelist";
            }

            return button;
        }
    }
}
