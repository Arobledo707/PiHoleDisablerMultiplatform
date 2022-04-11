using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Renderer;
using PiHoleDisablerMultiplatform.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace PiHoleDisablerMultiplatform
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Application.Current.RequestedThemeChanged += (OnThemeChange);

            DependencyService.Register<MockDataStore>();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.json");
            if (File.Exists(path))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new EnterInfoPage();
            }
        }

        private void OnThemeChange(Object sender, AppThemeChangedEventArgs a)
        {
            OSAppTheme theme = a.RequestedTheme;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case OSAppTheme.Dark:
                        mergedDictionaries.Add(new GreenDark());
                        break;
                    case OSAppTheme.Light:
                    default:
                        mergedDictionaries.Add(new GreenLight());
                        break;
                }

                var statusBar = DependencyService.Get<IStatusBar>();
                Color color = (Color)Application.Current.Resources["NavigationBarColor"];
                statusBar.SetStatusBarColor(color.ToHex());
            }
        }

        private void SetTheme()
        {
            OSAppTheme theme = Application.Current.RequestedTheme;
            if (Application.Current.UserAppTheme != theme)
            {
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();
                    switch (theme)
                    {
                        case OSAppTheme.Dark:
                            mergedDictionaries.Add(new GreenDark());
                            break;
                        case OSAppTheme.Light:
                        default:
                            mergedDictionaries.Add(new GreenLight());
                            break;
                    }
                    var statusBar = DependencyService.Get<IStatusBar>();
                    Color color = (Color)Application.Current.Resources["NavigationBarColor"];
                    statusBar.SetStatusBarColor(color.ToHex());
                }
            }
        }
        private async Task<bool> DataEntered()
        {
            CurrentPiData.piHoleData = await PiholeDataSerializer.DeserializeData();
            return await Task.FromResult(CurrentPiData.piHoleData.Token != null && CurrentPiData.piHoleData.Token != String.Empty);
        }

        protected override void OnStart()
        {
            SetTheme();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            SetTheme();
        }
    }
}
