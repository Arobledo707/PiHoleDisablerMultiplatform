using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using PiHoleDisablerMultiplatform.Renderer;

[assembly: Dependency(typeof(PiHoleDisablerMultiplatform.iOS.Renderer.iOSStatusBar))]
namespace PiHoleDisablerMultiplatform.iOS.Renderer
{
    public class iOSStatusBar : IStatusBar
    {
        public void SetStatusBarColor(string hexColor)
        {
            throw new NotImplementedException();
        }
    }
}