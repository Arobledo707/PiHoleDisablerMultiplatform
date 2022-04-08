using System;
using System.Collections.Generic;
using System.Text;
using PiHoleDisablerMultiplatform.Models;

namespace PiHoleDisablerMultiplatform.StaticPi
{
    public static class CurrentPiData
    {
        public static PiHoleData piHoleData { get; set; }

        public static bool DemoMode { get; set; }

        public static List<List<string>> demoData = new List<List<string>> { 
         new List<string> { "2022-03-21 19:37:26", "AAAA", "connectivitycheck.gstatic.com", "10.0.0.43", "3"},
         new List<string> { "2022-03-21 19:37:25", "A", "12.client-channel.google.com", "10.0.0.43", "3" },
         new List<string> { "2022-03-21 19:37:23", "A", "mail-ads.google.com", "10.0.0.53", "1" },
         new List<string> { "2022-03-21 19:37:13", "AAAA", "apple.com", "10.0.0.53", "2" },
         new List<string> { "2022-03-21 19:36:25", "A", "12.client-channel.google.com", "10.0.0.53", "2" },
         new List<string> { "2022-03-21 19:36:26", "A", "mail-ads.google.com", "10.0.0.53", "1" },
         new List<string> { "2022-03-21 19:36:13", "AAAA", "apple.com", "10.0.0.4", "2" },        
         new List<string> { "2022-03-21 19:35:26", "A", "mail-ads.google.com", "10.0.0.53", "1" },
         new List<string> { "2022-03-21 19:35:13", "AAAA", "apple.com", "10.0.0.4", "2" },
         new List<string> { "2022-03-21 19:35:25", "A", "12.client-channel.google.com", "10.0.0.4", "2" },
         new List<string> { "2022-03-21 19:34:26", "A", "mail-ads.google.com", "10.0.0.4", "1" },
         new List<string> { "2022-03-21 19:33:13", "AAAA", "apple.com", "10.0.0.4", "2" },
         new List<string> { "2022-03-21 19:32:26", "A", "mail-ads.google.com", "10.0.0.35", "1" },
         new List<string> { "2022-03-21 19:31:13", "AAAA", "apple.com", "10.0.0.35", "2" },
         new List<string> { "2022-03-21 19:30:25", "A", "12.client-channel.google.com", "10.0.0.43", "2" },
         new List<string> { "2022-03-21 19:29:26", "A", "mail-ads.google.com", "10.0.0.43", "4" },
         new List<string> { "2022-03-21 19:28:13", "AAAA", "apple.com", "10.0.0.33", "2" },
         new List<string> { "2022-03-21 19:27:26", "A", "ads.google.com", "10.0.0.33", "4" },
         new List<string> { "2022-03-21 19:26:13", "AAAA", "apple.com", "10.0.0.12", "2" },
         new List<string> { "2022-03-21 19:25:25", "A", "12.client-channel.google.com", "10.0.0.66", "2" },
         new List<string> { "2022-03-21 19:24:26", "A", "mail-ads.google.com", "10.0.0.66", "4" },
         new List<string> { "2022-03-21 19:23:13", "AAAA", "apple.com", "10.0.0.33", "2" },
         new List<string> { "2022-03-21 19:22:26", "A", "facebook.com", "10.0.0.32", "1" },
         new List<string> { "2022-03-21 19:21:13", "AAAA", "apple.com", "10.0.0.33", "2" },
         new List<string> { "2022-03-21 19:20:25", "A", "ssl.google-analytics.com", "10.0.0.43", "1" },
         new List<string> { "2022-03-21 19:19:26", "AAAA", "ssl.google-analytics.com", "10.0.0.43", "1" },
         new List<string> { "2022-03-21 19:18:13", "AAAA", "apple.com", "10.0.0.66", "2" },
         new List<string> { "2022-03-21 19:17:26", "A", "mail-ads.google.com", "10.0.0.33", "1" },
         new List<string> { "2022-03-21 19:16:13", "AAAA", "apple.com", "10.0.0.33", "2" },
         new List<string> { "2022-03-21 19:15:25", "A", "12.client-channel.google.com", "10.0.0.57", "2" },
         new List<string> { "2022-03-21 19:14:26", "A", "mail-ads.google.com", "10.0.0.57", "1" },};

        static CurrentPiData() 
        {
            piHoleData = new PiHoleData();
            piHoleData.Url = String.Empty;
            piHoleData.Token = String.Empty;
        }
    }
}
