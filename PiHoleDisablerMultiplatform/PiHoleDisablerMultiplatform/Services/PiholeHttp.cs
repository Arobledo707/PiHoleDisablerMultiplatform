using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Net.NetworkInformation;

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
                    //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Content.ReadAsStringAsync().Result);
                    //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Content.ReadAsStringAsync().Result);
                    var content = message.Content;
                    var contentString = await message.Content.ReadAsStringAsync();
                    var geader = message.Content.Headers;
                    //var contentDict = JsonConvert.DeserializeObject<List<string>>(contentString);
                    //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Content.ReadAsStringAsync().Result);
                    return "values";
                }
            }
            catch (Exception err) 
            {
                Console.WriteLine(err);
                return "notok";
            }
            return "ok";
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
