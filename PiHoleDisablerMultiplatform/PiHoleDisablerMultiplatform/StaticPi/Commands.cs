using System;
using System.Collections.Generic;
using System.Text;

namespace PiHoleDisablerMultiplatform.StaticPi
{
    public static class Constants
    {
        public enum Theme
        {
            Default,
            Blue,
            Green,
            Orange,
            Purple,
            Grey
        };

        public const string error = "error";
        public const string validInfo = "validInfo";
        public const string checkInfo = "checkInfo";
        public const string infoRequest = "requestInfo";
        public const string requestedData = "requestedData";
        public const string clear = "clear";
        public const string statusUpdate = "statusUpdate";
        public const string refresh = "refresh";
        public const string listChange = "listChange";

        public const string cancel = "cancel";
    }
}
