using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace PiHoleDisablerMultiplatform
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Application.Current.RequestedThemeChanged += (OnThemeChange);

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        private void OnThemeChange(Object sender, AppThemeChangedEventArgs a) 
        {
            OSAppTheme theme = a.RequestedTheme;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            //var test = mergedDictionaries.
            
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case OSAppTheme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case OSAppTheme.Light:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
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
                            mergedDictionaries.Add(new DarkTheme());
                            break;
                        case OSAppTheme.Light:
                        default:
                            mergedDictionaries.Add(new LightTheme());
                            break;
                    }
                }
            }
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
            //if (Application.Current.UserAppTheme != Application.Current.RequestedTheme) 
            //{
            //    bool lol = true;
            //}
            //OSAppTheme currentTheme = Application.Current.RequestedTheme;
        }
    }
}
