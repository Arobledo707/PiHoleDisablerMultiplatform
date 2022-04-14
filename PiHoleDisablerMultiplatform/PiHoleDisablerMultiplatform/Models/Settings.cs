using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.Models
{
    public class Settings
    {
        public Settings() 
        {
            Theme = Constants.Theme.Default;
            QueryCount = Constants.k30Queries;
        }
        public int QueryCount { get; set; }
        public Constants.Theme Theme { get; set; }
    }
}
