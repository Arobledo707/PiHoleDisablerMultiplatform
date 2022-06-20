﻿using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.Views;
using PiHoleDisablerMultiplatform.Models;
using PiHoleDisablerMultiplatform.StaticPi;
using System.Collections.Generic;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private const string kChooseTheme = "Choose Theme";
        public Command ThemeCommand { get; }
        public SettingsViewModel() 
        {
            ThemeCommand = new Command(ThemeButtonClicked);
        }

        private async void ThemeButtonClicked(object obj) 
        {
            Page page = obj as Page;
            var result = await page.DisplayActionSheet(kChooseTheme, Constants.cancel, null,
     Constants.Theme.Default.ToString(), Constants.Theme.Blue.ToString(), Constants.Theme.Green.ToString(),
     Constants.Theme.Orange.ToString(), Constants.Theme.Purple.ToString());

            if (result != null)
            {
                UI.ThemeButton themeButton = page.FindByName<UI.ThemeButton>("themeButton");
                if (themeButton != null) 
                {
                    themeButton.Text = result;
                }
                DetectThemeChoice(result);
            }
        }

        private async void DetectThemeChoice(string themeString)
        {
            if (themeString == Constants.cancel)
            {
                return;
            }
            else if (themeString == Constants.Theme.Default.ToString())
            {
                CurrentPiData.CurrentSettings.Theme = Constants.Theme.Default;
            }
            else if (themeString == Constants.Theme.Blue.ToString())
            {
                CurrentPiData.CurrentSettings.Theme = Constants.Theme.Blue;
            }
            else if (themeString == Constants.Theme.Green.ToString())
            {
                CurrentPiData.CurrentSettings.Theme = Constants.Theme.Green;
            }
            else if (themeString == Constants.Theme.Orange.ToString())
            {
                CurrentPiData.CurrentSettings.Theme = Constants.Theme.Orange;
            }
            else if (themeString == Constants.Theme.Purple.ToString())
            {
                CurrentPiData.CurrentSettings.Theme = Constants.Theme.Purple;
            }
            else if (themeString == Constants.Theme.Grey.ToString())
            {
                CurrentPiData.CurrentSettings.Theme = Constants.Theme.Grey;
            }
            App app = Application.Current as App;
            app.SetThemes();
            app.ChangeTheme();

            bool result = await Serializer.SerializeDataAsync(CurrentPiData.CurrentSettings, Constants.kSettingsFile);
            if (!result)
            {
                Console.WriteLine("Error: could not save settings");
            }
        }
    }
}
