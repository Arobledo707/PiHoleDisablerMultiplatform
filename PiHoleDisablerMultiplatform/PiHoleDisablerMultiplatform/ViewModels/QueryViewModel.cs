using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Models;
using PiHoleDisablerMultiplatform.UI;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class QueryViewModel : BaseViewModel
    {
        private const string kBlackListed = "5";
        private const string kGravity = "1";
        private const string kPageTitle = "Queries";
        private const string kWhitelist = "whitelist";
        private const string kBlacklist = "blacklist";
        private const double kDateTimeStacklayoutWidth = 60.0f;
        private const double kTypeStacklayoutWidth = 50;
        private const double kDomainStacklayoutWidth = 120;
        private const double kClientStacklayoutWidth = 80;
        private const double kDefaultRightThickness = 10;
        private const int kSmallFontSize = 11;
        private const int kDateTimeIndex = 0;
        private const int kTypeIndex = 1;
        private const int kDomainIndex = 2;
        private const int kClientIndex = 3;
        private const int kStatusIndex = 4;

        private int queryCount = Constants.k30Queries;


        private bool isRefreshing = false;
        public bool IsCurrentlyRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        public Command RefreshCommand { get; }

        public Command WhiteListCommand { get; }
        public Command BlackListCommand { get; }

        public Command QueryCountCommand { get; }

        public int QueryCount { get { return queryCount; } }

        public QueryViewModel()
        {
            RefreshCommand = new Command(Refresh, canExecute:(object param) => { return !IsCurrentlyRefreshing; });
            WhiteListCommand = new Command(WhitelistButtonClick);
            BlackListCommand = new Command(BlacklistButtonClick);
            QueryCountCommand = new Command(ChangeQueryCount);

            if (CurrentPiData.CurrentSettings.QueryCount != 0) 
            {
                queryCount = CurrentPiData.CurrentSettings.QueryCount;
            }

            Title = kPageTitle;
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
            bool result = true;
            if (!CurrentPiData.DemoMode)
            {
                result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "black", "add", (string)param);
            }
            else 
            {
                DemoModeButtonClick((string)param, 1);
            }
            if (result)
            {
                if (!CurrentPiData.DemoMode)
                {
                    result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "white", "sub", (string)param);
                }
                if (!result)
                {
                    //
                }
                MessagingCenter.Send(this, Constants.listChange, new List<string> { "Success", $"Added {(string)param} to blacklist" });
            }
            else
            {
                MessagingCenter.Send(this, Constants.listChange, new List<string> { "Error", $"Failed to add {(string)param} to blacklist" });
            }
        }

        private async void WhitelistButtonClick(object param)
        {
            bool result = true;
            if (!CurrentPiData.DemoMode)
            {
                result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "white", "add", (string)param);
            }
            else 
            {
                DemoModeButtonClick((string)param, 2);
            }
            if (result)
            {
                if (!CurrentPiData.DemoMode)
                {
                    result = await PiholeHttp.PiHoleList(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, "black", "sub", (string)param);
                }
                if (!result)
                {
                    //
                }
                MessagingCenter.Send(this, Constants.listChange, new List<string> { "Success", $"Added {(string)param} to whitelist" });
            }
            else
            {
                MessagingCenter.Send(this, Constants.listChange, new List<string> { "Error", $"Failed to add {(string)param} to whitelist" });

            }
        }

        private void DemoModeButtonClick(string domain, int changePermission) 
        {
            foreach (List<string> entry in CurrentPiData.demoData) 
            {
                if (domain == entry[2]) 
                {
                    entry[4] = changePermission.ToString();
                }
            }
        }

        private async void Refresh(object param) 
        {
            IsCurrentlyRefreshing = true;

            await Task.Run(async () => { await RefreshTask(param); });

            IsCurrentlyRefreshing  = false;
            
        }

        private async void ChangeQueryCount(object obj) 
        {
            Page page = obj as Page;
            string selection = await page.DisplayActionSheet("Query Count:", Constants.cancel, null, Constants.k10Queries.ToString(), 
                Constants.k30Queries.ToString(), Constants.k50Queries.ToString(), Constants.k100Queries.ToString());

            if (selection != Constants.cancel) 
            {
                queryCount = Convert.ToInt32(selection);
                OnPropertyChanged("QueryCount");
                CurrentPiData.CurrentSettings.QueryCount = queryCount;
                bool result =  await Serializer.SerializeDataAsync(CurrentPiData.CurrentSettings, Constants.kSettingsFile);
                if(!result)
                {
                    await page.DisplayAlert("IO Error:", "Could not save settings", "Ok");
                }
            }

        }


        private async Task<bool> RefreshTask(object param) 
        {
            string contentString = String.Empty;
            if (!CurrentPiData.DemoMode)
            {
                contentString = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, queryCount);
            }


            if ((contentString == String.Empty || contentString == null) && !CurrentPiData.DemoMode)
            {
                return await Task.FromResult(false);
            }

            StackLayout content = ReturnStackLayout(param);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                View labelInfo = content.Children[0];
                content.Children.Clear();

                content.Children.Add(labelInfo);
            });

            Queue<StackLayout> stackLayoutQueue = new Queue<StackLayout>();

            try
            {
                QueryData queryData = new QueryData();
                if (!CurrentPiData.DemoMode)
                {
                    queryData = JsonConvert.DeserializeObject<QueryData>(contentString);
                }
                else
                {
                    if (queryCount > Constants.k30Queries)
                    {
                        queryData.data = CurrentPiData.demoData;
                    }
                    else 
                    {
                        queryData.data = CurrentPiData.demoData.GetRange(0, queryCount);
                    }
                }
                var enumerate = Application.Current.Resources.MergedDictionaries.GetEnumerator();
                enumerate.MoveNext();
                ResourceDictionary currentTheme = enumerate.Current;
                StackLayout replacementContent = new StackLayout();

                for(int i = queryData.data.Count -1; i >= 0; --i)
                {
                    List<string> stringList = queryData.data[i];
                    bool allowed = false;
                    if (stringList[kStatusIndex] != kGravity && stringList[kStatusIndex] !=kBlackListed)
                    {
                        allowed = true;
                    }

                    StackLayout stackLayout = CreateStackLayout(allowed);
                    for (int j = 0; j < kStatusIndex; ++j)
                    {
                        double widthRequest = 0;
                        double fontSize = 12;
                        string text = stringList[j];
                        var layoutOption = LayoutOptions.FillAndExpand;
                        switch (j)
                        {
                            case kDateTimeIndex:
                                widthRequest = kDateTimeStacklayoutWidth;
                                try
                                {
                                    long epoch = long.Parse(text);
                                    DateTimeOffset dtOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(text));
                                    text = dtOffset.DateTime.ToLocalTime().ToString();
                                }
                                catch (Exception err)
                                {
                                    Console.Error.WriteLine("error: " + err + err.Message);
                                }
                                fontSize = kSmallFontSize;
                                break;
                            case kTypeIndex:
                                widthRequest = kTypeStacklayoutWidth;
                                fontSize = kSmallFontSize;
                                break;
                            case kDomainIndex:
                                widthRequest = kDomainStacklayoutWidth;
                                layoutOption = LayoutOptions.CenterAndExpand;
                                break;
                            case kClientIndex:
                                widthRequest = kClientStacklayoutWidth;
                                layoutOption = LayoutOptions.EndAndExpand;
                                break;
                        }
                        stackLayout.Children.Add(CreateLabel(text, fontSize, widthRequest, layoutOption));
                    }
                    stackLayout.Children.Add(CreateButton(allowed, stringList[kDomainIndex]));
                    stackLayoutQueue.Enqueue(stackLayout);
                }

                MainThread.BeginInvokeOnMainThread(() => 
                {
                    while (stackLayoutQueue.Count > 0) 
                    {
                        content.Children.Add(stackLayoutQueue.Dequeue());
                    }
                });
                IsCurrentlyRefreshing = false;
                return await Task.FromResult(true);
            }
            catch (Exception err)
            {
                Console.WriteLine(err + ": " + err.Message);
                IsCurrentlyRefreshing = false;
                return await Task.FromResult(false);
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
            label.Padding = new Thickness(0, 0, kDefaultRightThickness, 0);
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
                button.Text = kWhitelist;
                button.Command = WhiteListCommand;
            }
            else
            {
                button = new BlacklistButton();
                button.Text = kBlacklist;
                button.Command = BlackListCommand;
            }
            button.FontSize = kSmallFontSize;
            button.HorizontalOptions = LayoutOptions.EndAndExpand;
            button.CommandParameter = parameter;

            return button;
        }
    }
}
