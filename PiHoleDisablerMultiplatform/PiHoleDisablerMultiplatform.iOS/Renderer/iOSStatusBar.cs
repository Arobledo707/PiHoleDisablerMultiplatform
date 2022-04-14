using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PiHoleDisablerMultiplatform.Renderer;

[assembly: Dependency(typeof(PiHoleDisablerMultiplatform.iOS.Renderer.iOSStatusBar))]
namespace PiHoleDisablerMultiplatform.iOS.Renderer
{
    public class iOSStatusBar : IStatusBar
    {
        public void SetStatusBarColor(string hexColor)
        {
            UIView statusBar = new UIView(UIApplication.SharedApplication.StatusBarFrame);
            if (statusBar != null && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = Color.FromHex(hexColor).ToUIColor();
            }
        }
    }
}