using System;
using System.Collections.Generic;
using System.Text;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.ViewModels
{
    public class QueryViewModel : BaseViewModel
    {
        public QueryViewModel()
        {
            Title = "Queries";
            Test();
        }

        private async void Test() 
        {
            string test = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, 2);
            if (test == String.Empty) 
            {
                Console.WriteLine("CLEAR");
            }
        }
    }
}
