using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Net.NetworkInformation;
using PiHoleDisablerMultiplatform.Models;

namespace PiHoleDisablerMultiplatform.Services
{
    public static class PiholeHttp
    {
        public enum Command 
        {
            Enable,
            Disable,
            Invalid
        };

        public async static Task<string> GetQueries(string savedUrl, string currentApiToken, int count) 
        {
            try
            {
                //http://pi.hole/admin/queries.php
                string url = $"http://{savedUrl}/admin/api.php?getAllQueries={count}&auth={currentApiToken}";
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var message = await client.GetAsync(url);
                //string test = await client.GetStringAsync(url);
                if (message.IsSuccessStatusCode)
                {
                    var contentString = await message.Content.ReadAsStringAsync();
                    //var values = JsonConvert.DeserializeObject<QueryData>(contentString);
                
                    return contentString;
                }
            }
            catch (Exception err) 
            {
                Console.WriteLine(err);
                return String.Empty;
            }
            return String.Empty;
        }

        public async static Task<string> CheckPiholeStatus(string savedUrl, string currentApiToken) 
        {
            try
            {
                string url = $"http://{savedUrl}/admin/api.php?status&auth={currentApiToken}";
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var message = await client.GetAsync(url);
                if (message.IsSuccessStatusCode)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Content.ReadAsStringAsync().Result);
                    return values["status"];
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err + err.Message);
                //Toast.MakeText(this, err + err.Message, ToastLength.Short).Show();
            }
            return "disconnected";

        }

        public async static Task<bool> AddToList(string savedUrl, string currentApiToken, string list, string action, string domain) 
        {
            try
            {
                //add/sub for actions
                // list is black, white, regex_white and regex_black.
                //Add API endpoints api.php? list = black & add = domain.com like commands
                //where add = domain and sub = domain can be used to manage the lists.A simple api.php? list = black will simply list all blacklisted domains.
                string url = $"http://{savedUrl}/admin/api.php?list={list}&action={action}={domain}&auth={currentApiToken}";
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var message = await client.GetAsync(url);
                HttpResponseMessage sendMessage;
                //sendMessage = HttpContent.
                //client.SendAsync(url);
                if (message.IsSuccessStatusCode) 
                {
                    var content = await message.Content.ReadAsStringAsync();

                    return await Task.FromResult(true);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err + ": " + err.Message);
            }

            return await Task.FromResult(false);
        }

        public async static Task<bool> PiholeCommand(string savedUrl, string currentApiToken, string command, int timeInSeconds) 
        {
            try
            {
                string commandString = command;
                if (command.StartsWith("d")) 
                {
                    commandString += '=';
                    commandString += timeInSeconds.ToString();
                }
                string url = $"http://{savedUrl}/admin/api.php?{commandString}&auth={currentApiToken}";
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var message = await httpClient.GetAsync(url);
                if (message.IsSuccessStatusCode) 
                {
                    var content = await message.Content.ReadAsStringAsync();
                    if (content.Contains("status")) 
                    {
                        return await Task.FromResult(true);
                    }
                }

            }
            catch(Exception err)
            {
                Console.WriteLine(err + err.Message);
            }
            return await Task.FromResult(false);
        }

    }

}
