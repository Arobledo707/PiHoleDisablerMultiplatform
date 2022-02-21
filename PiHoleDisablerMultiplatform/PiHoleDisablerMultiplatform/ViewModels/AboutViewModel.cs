using PiHoleDisablerMultiplatform.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Net.Http;


namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public Command SaveInfoCommand { get; }
        Entry tokenEntered;
        public AboutViewModel()
        {
            Title = "Pi-Hole Disabler";
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            SaveInfoCommand = new Command(OnSaveButtonClicked);
            tokenEntered = Shell.Current.CurrentPage.FindByName<Entry>("tokenEnteredText");
        }
        private async void OnSaveButtonClicked(object obj) 
        {
            //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            //await Shell.Current.GoToAsync($"//{nameof(DisablePage)}");
            var test = Shell.Current.CurrentPage.FindByName<Label>("enterPiholeInfo");
            Console.WriteLine(test.Text);
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var message = await client.GetAsync("http://pi.hole//admin/api.php?status&auth=0740ca623ab64bea722d50e694a2d51200351b5775cc429dd86598f7ac71bbf8");
            if (message.IsSuccessStatusCode) 
            {

                string lol = tokenEntered.Text;
                Console.WriteLine(lol);
            }
        }
        //public ICommand OpenWebCommand { get; }
    }
}