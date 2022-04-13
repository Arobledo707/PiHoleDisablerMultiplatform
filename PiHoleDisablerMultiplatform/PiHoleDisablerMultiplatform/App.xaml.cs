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
        private ResourceDictionary lightTheme = new RedLight();
        private ResourceDictionary darkTheme = new RedDark();
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
                        mergedDictionaries.Add(darkTheme);
                        break;
                    case OSAppTheme.Light:
                    default:
                        mergedDictionaries.Add(lightTheme);
                        break;
                }

                var statusBar = DependencyService.Get<IStatusBar>();
                Color color = (Color)Application.Current.Resources["NavigationBarColor"];
                statusBar.SetStatusBarColor(color.ToHex());
            }
        }

        public void SetThemes() 
        {
            switch (CurrentPiData.currentTheme) 
            {
                case Constants.Theme.Purple:
                    lightTheme = new PurpleLight();
                    darkTheme = new PurpleDark();
                    break;
                case Constants.Theme.Blue:
                    lightTheme = new BlueLight();
                    darkTheme = new BlueDark();
                    break;
                case Constants.Theme.Green:
                    lightTheme = new GreenLight();
                    darkTheme = new GreenDark();
                    break;
                case Constants.Theme.Orange:
                    lightTheme = new OrangeLight();
                    darkTheme = new OrangeDark();
                    break;
                case Constants.Theme.Grey:
                    lightTheme = new GreyLight();
                    darkTheme = new GreyDark();
                    break;
                case Constants.Theme.Default:
                    lightTheme = new RedLight();
                    darkTheme = new RedDark();
                    break;
            }
        }

        public void ChangeTheme()
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
                            mergedDictionaries.Add(darkTheme);
                            break;
                        case OSAppTheme.Light:
                        default:
                            mergedDictionaries.Add(lightTheme);
                            break;
                    }
                    var statusBar = DependencyService.Get<IStatusBar>();
                    Color color = (Color)Application.Current.Resources["NavigationBarColor"];
                    statusBar.SetStatusBarColor(color.ToHex());
                }
            }
        }

        protected override void OnStart()
        {
            ChangeTheme();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            ChangeTheme();
        }
    }
}
