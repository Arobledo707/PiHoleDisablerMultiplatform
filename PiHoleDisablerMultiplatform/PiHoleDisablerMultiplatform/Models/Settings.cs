using PiHoleDisablerMultiplatform.StaticPi;

namespace PiHoleDisablerMultiplatform.Models
{
    public class Settings
    {
        public Settings()
        {
            Theme = Constants.Theme.Default;
            QueryCount = Constants.k30Queries;
            OnlyShowTime = false;
            TwentyFourHourTime = false;
            DayMonthYear = false;
            

        }
        public int QueryCount { get; set; }
        public Constants.Theme Theme { get; set; }

        public bool OnlyShowTime { get; set; }

        public bool TwentyFourHourTime { get; set; }

        public bool DayMonthYear { get; set; }
    }
}
