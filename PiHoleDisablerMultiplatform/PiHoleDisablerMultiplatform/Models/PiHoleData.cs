using System;
using System.Collections.Generic;
using System.Text;

namespace PiHoleDisablerMultiplatform.Models
{
    public class PiHoleData
    {

        public PiHoleData(string name, string url, string token) 
        {
            Name = name;
            Url = url;
            Token = token;
        }

        public PiHoleData() 
            :this("Pi-hole", "http://pi.hole", "")
        {
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
    }
}
