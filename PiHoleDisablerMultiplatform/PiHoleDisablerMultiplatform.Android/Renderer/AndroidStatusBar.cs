using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Android;
using PiHoleDisablerMultiplatform.Renderer;

[assembly: Dependency(typeof(PiHoleDisablerMultiplatform.Droid.Renderer.AndroidStatusBar))]

namespace PiHoleDisablerMultiplatform.Droid.Renderer
{
    public class AndroidStatusBar : IStatusBar
    {
        public void SetStatusBarColor(string hexColor)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) 
            {
                Window window = Platform.CurrentActivity.Window;
                window.SetStatusBarColor(Android.Graphics.Color.ParseColor(hexColor));
                
            }
        }
    }
}