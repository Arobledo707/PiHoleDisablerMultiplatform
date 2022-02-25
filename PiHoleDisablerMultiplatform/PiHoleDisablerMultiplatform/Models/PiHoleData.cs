using System;
using System.Collections.Generic;
using System.Text;

namespace PiHoleDisablerMultiplatform.Models
{
    public class PiHoleData
    {
        public PiHoleData() 
        {

        }
        public PiHoleData(string url, string token) 
        {
            Url = url;
            Token = token;
        }


        public string Url { get; set; }
        public string Token { get; set; }
    }
}
