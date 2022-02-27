using System;
using System.Collections.Generic;
using System.Text;
using PiHoleDisablerMultiplatform.Models;

namespace PiHoleDisablerMultiplatform.StaticPi
{
    public static class CurrentPiData
    {
        public static PiHoleData piHoleData { get; set; }

        static CurrentPiData() 
        {
            piHoleData = new PiHoleData();
            piHoleData.Url = String.Empty;
            piHoleData.Token = String.Empty;
        }
    }
}
