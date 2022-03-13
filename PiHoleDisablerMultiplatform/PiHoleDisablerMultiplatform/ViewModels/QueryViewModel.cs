using System;
using System.Collections.Generic;
using System.Text;
using PiHoleDisablerMultiplatform.Services;
using PiHoleDisablerMultiplatform.StaticPi;
using PiHoleDisablerMultiplatform.Models;
using Newtonsoft.Json;

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
            string contentString = await PiholeHttp.GetQueries(CurrentPiData.piHoleData.Url, CurrentPiData.piHoleData.Token, 10);
            if (contentString == String.Empty || contentString == null) 
            {
                return;
            }
            QueryData data = JsonConvert.DeserializeObject<QueryData>(contentString);

        }
    }
}
