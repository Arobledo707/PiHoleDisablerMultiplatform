using System;
using System.Collections.Generic;
using System.Text;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Models;
using PiHoleDisablerMultiplatform.UI;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class QueryViewModel : BaseViewModel
    {
        private const string blackListed = "5";
        private bool isRefreshing = false;
        public bool IsCurrentlyRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        public Command RefreshCommand { get; }

        public Command WhiteListCommand { get; }
        public Command BlackListCommand { get; }

        public QueryViewModel()
        {
            RefreshCommand = new Command(Refresh);
            WhiteListCommand = new Command(WhitelistButtonClick);
            BlackListCommand = new Command(BlacklistButtonClick);

            Title = "Queries";
        }


        private StackLayout ReturnStackLayout(object param)
        {
            ContentPage page = param as ContentPage;
            ScrollView scrollView;
            if (page != null)
            {
                scrollView = page.Content.FindByName<ScrollView>("scrollView");
                StackLayout stackLayout = (StackLayout)scrollView.Content;
                return stackLayout;
            }
            scrollView = param as ScrollView;
            if (scrollView != null)
            {
                return scrollView.Content as StackLayout;
            }
            return null;
        }

        private async void BlacklistButtonClick(object param)
        {
            bool result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "black", "add", (string)param);
            if (result)
            {
                result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "white", "sub", (string)param);
                if (!result)
                {
                    //
                }
                MessagingCenter.Send(this, Commands.listChange, new List<string> { "Success", $"Added {(string)param} to blacklist" });
            }
            else
            {
                MessagingCenter.Send(this, Commands.listChange, new List<string> { "Error", $"Failed to add {(string)param} to blacklist" });
            }
        }

        private async void WhitelistButtonClick(object param)
        {
            bool result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "white", "add", (string)param);
            if (result)
            {
                result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "black", "sub", (string)param);
                if (!result)
                {
                    //
                }
                MessagingCenter.Send(this, Commands.listChange, new List<string> { "Success", $"Added {(string)param} to whitelist" });
            }
            else
            {
                MessagingCenter.Send(this, Commands.listChange, new List<string> { "Error", $"Failed to add {(string)param} to whitelist" });

            }
        }

        private async void Refresh(object param)
        {
            IsCurrentlyRefreshing = true;
            string contentString = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, 30);
            StackLayout content = ReturnStackLayout(param);
            // keep item title 
            var labelInfo = content.Children[0];
            content.Children.Clear();
            content.Children.Add(labelInfo);

            if (contentString == String.Empty || contentString == null)
            {
                return;
            }
            Console.Error.WriteLine("Content String is not null/empty");
            try
            {
                QueryData queryData = JsonConvert.DeserializeObject<QueryData>(contentString);
                var enumerate = Application.Current.Resources.MergedDictionaries.GetEnumerator();
                enumerate.MoveNext();
                ResourceDictionary currentTheme = enumerate.Current;

                foreach (List<string> stringList in queryData.data)
                {
                    bool allowed = false;
                    if (stringList[4] != "1" && stringList[4] != blackListed)
                    {
                        allowed = true;
                    }
                    StackLayout stackLayout = CreateStackLayout(allowed);
                    for (int i = 0; i < 4; ++i)
                    {
                        double widthRequest = 0;
                        double fontSize = 12;
                        string text = stringList[i];
                        var layoutOption = LayoutOptions.FillAndExpand;
                        switch (i)
                        {
                            case 0:
                                widthRequest = 60;
                                try
                                {
                                    long epoch = long.Parse(text);
                                    DateTimeOffset dtOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(text));
                                    DateTime dt = dtOffset.DateTime;
                                    text = dtOffset.DateTime.ToString();
                                }
                                catch (Exception err) 
                                {
                                    Console.Error.WriteLine("error: " + err + err.Message);
                                }
                                fontSize = 11;
                                break;
                            case 1:
                                widthRequest = 50;
                                fontSize = 11;
                                break;
                            case 2:
                                widthRequest = 120;
                                layoutOption = LayoutOptions.CenterAndExpand;
                                break;
                            case 3:
                                widthRequest = 80;
                                layoutOption = LayoutOptions.EndAndExpand;
                                break;
                            case 4:
                                widthRequest = 50;
                                break;
                        }
                        stackLayout.Children.Add(CreateLabel(text, fontSize, widthRequest, layoutOption));
                    }
                    stackLayout.Children.Add(CreateButton(allowed, stringList[2]));
                    content.Children.Add(stackLayout);
                }
                IsCurrentlyRefreshing = false;
            }
            catch (Exception err) 
            {
                Console.WriteLine(err + ": " + err.Message);
                IsCurrentlyRefreshing = false;
            }
        }

        private StackLayout CreateStackLayout(bool allowed)
        {
            StackLayout stackLayout;
            if (allowed)
            {
                stackLayout = new AllowedQueryStackLayout();
            }
            else 
            {
                stackLayout = new BlockedQueryStackLayout();
            }
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            stackLayout.FlowDirection = FlowDirection.LeftToRight;

            return stackLayout;
        }

        private Label CreateLabel(string text, double fontSize, double widthRequest, LayoutOptions options)
        {
            Label label = new DynamicLabel();
            label.HorizontalOptions = options;
            label.WidthRequest = widthRequest;
            label.Padding = new Thickness(0, 0, 10, 0);
            label.FontSize = fontSize;
            label.Text = text;
            return label;
        }

        private Button CreateButton(bool allowed, string parameter)
        {

            Button button;

            if (!allowed)
            {
                button = new WhitelistButton();
                button.Text = "blacklist";
                button.Command = BlackListCommand;
            }
            else
            {
                button = new BlacklistButton();
                button.Text = "whitelist";
                button.Command = WhiteListCommand;
            }
            button.FontSize = 11;
            button.HorizontalOptions = LayoutOptions.EndAndExpand;
            button.CommandParameter = parameter;

            return button;
        }
    }
}
