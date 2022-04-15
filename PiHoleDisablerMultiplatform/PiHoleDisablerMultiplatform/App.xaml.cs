using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Renderer;
using PiHoleDisablerMultiplatform.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace PiHoleDisablerMultiplatform
{
    public partial class App : Application
    {
        private ResourceDictionary lightTheme = new RedLight();
        private ResourceDictionary darkTheme = new RedDark();
        public App()
        {
            InitializeComponent();
            LoadSettings();
            Application.Current.RequestedThemeChanged += (OnThemeChange);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.kPiDataFile);
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
            switch (CurrentPiData.CurrentSettings.Theme) 
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

        private async void LoadSettings() 
        {
            Settings settings =  await Serializer.DeserializeSettingsDataAsync() as Settings;
            if (settings != null) 
            {
                CurrentPiData.CurrentSettings = settings;
                SetThemes();
                ChangeTheme();
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
