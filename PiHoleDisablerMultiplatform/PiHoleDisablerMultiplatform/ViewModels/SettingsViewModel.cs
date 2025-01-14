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
        public Command Check24HourCommand { get; }
        public Command CheckTimeOnlyCommand { get; }
        public Command DateFormatCommand { get; }
        public Command OnAppearingCommand { get; }
        public Command QueryCountCommand { get; }

        public SettingsViewModel() 
        {
            ThemeCommand = new Command(ThemeButtonClicked);
            Check24HourCommand = new Command(TwentyFourHourTime);
            CheckTimeOnlyCommand = new Command(ShowTimeOnly);
            DateFormatCommand = new Command(DateFormat);
            OnAppearingCommand = new Command(OnAppear);
            QueryCountCommand = new Command(QueryCountClicked);
        }

        private async void ThemeButtonClicked(object obj) 
        {
            Page page = obj as Page;
            var result = await page.DisplayActionSheet(kChooseTheme, Constants.cancel, null,
            Constants.Theme.Default.ToString(), Constants.Theme.Blue.ToString(), Constants.Theme.Green.ToString(),
            Constants.Theme.Orange.ToString(), Constants.Theme.Purple.ToString(), Constants.Theme.Grey.ToString());

            if (result != null)
            {
                DetectThemeChoice(result);
                UI.ThemeButton themeButton = page.FindByName<UI.ThemeButton>("themeButton");
                if (themeButton != null)
                {
                    themeButton.Text = CurrentPiData.CurrentSettings.Theme.ToString();
                }
            }
        }

        private async void QueryCountClicked(object obj) 
        {
            Page page = obj as Page;
            string selection = await page.DisplayActionSheet("Query Count:", Constants.cancel, null, Constants.k10Queries.ToString(),
                Constants.k30Queries.ToString(), Constants.k50Queries.ToString(), Constants.k100Queries.ToString());

            if (selection != Constants.cancel)
            {
                UI.ThemeButton queryButton = page.FindByName<UI.ThemeButton>("queryCountButton");
                if (queryButton != null) 
                {
                    int queryCount = Convert.ToInt32(selection);
                    queryButton.Text = selection;
                    CurrentPiData.CurrentSettings.QueryCount = queryCount;

                    bool result = await Serializer.SerializeDataAsync(CurrentPiData.CurrentSettings, Constants.kSettingsFile);
                    if (!result)
                    {
                        await page.DisplayAlert("IO Error:", "Could not save settings", "Ok");
                    }
                }
            }



        }

        private void OnAppear(object obj) 
        {
            SettingsPage page = obj as SettingsPage;

            if (page != null) 
            {
                CheckBox timeBox = page.FindByName<CheckBox>("timeOnlyCheckbox");
                if (timeBox != null) 
                {
                    timeBox.IsChecked = CurrentPiData.CurrentSettings.OnlyShowTime;
                }

                CheckBox hourBox = page.FindByName<CheckBox>("hourCheckBox");
                if (hourBox != null) 
                {
                    hourBox.IsChecked = CurrentPiData.CurrentSettings.TwentyFourHourTime;
                }

                CheckBox dateFormatBox = page.FindByName<CheckBox>("dayMonthYear");
                if (dateFormatBox != null) 
                {
                    dateFormatBox.IsChecked = CurrentPiData.CurrentSettings.DayMonthYear;
                }

                Button themeButton = page.FindByName<Button>("themeButton");
                if (themeButton != null) 
                {
                    themeButton.Text = CurrentPiData.CurrentSettings.Theme.ToString();
                }

                Button queryCountButton = page.FindByName<Button>("queryCountButton");
                if (queryCountButton != null)
                {
                    queryCountButton.Text = CurrentPiData.CurrentSettings.QueryCount.ToString();
                }
            }
        }

        private async void TwentyFourHourTime(object obj) 
        {
            CheckBox checkBox = obj as CheckBox;
            if (checkBox != null) 
            {
                CurrentPiData.CurrentSettings.TwentyFourHourTime = checkBox.IsChecked;

                bool result = await Serializer.SerializeDataAsync(CurrentPiData.CurrentSettings, Constants.kSettingsFile);
                if (!result)
                {
                    Console.WriteLine("Error: could not save settings");
                }

            }

        }

        private async void ShowTimeOnly(object obj) 
        {
            CheckBox checkBox = obj as CheckBox;
            if (checkBox != null)
            {
                CurrentPiData.CurrentSettings.OnlyShowTime = checkBox.IsChecked;

                bool result = await Serializer.SerializeDataAsync(CurrentPiData.CurrentSettings, Constants.kSettingsFile);
                if (!result)
                {
                    Console.WriteLine("Error: could not save settings");
                }
            }
        }

        private async void DateFormat(object obj) 
        {
            CheckBox checkBox = obj as CheckBox;
            if (checkBox != null)
            {
                CurrentPiData.CurrentSettings.DayMonthYear = checkBox.IsChecked;

                bool result = await Serializer.SerializeDataAsync(CurrentPiData.CurrentSettings, Constants.kSettingsFile);
                if (!result)
                {
                    Console.WriteLine("Error: could not save settings");
                }

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
