using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace PiHoleDisablerMultiplatform.UI
{
    public class DynamicTabBar : TabBar
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != null) 
            {
                string what = propertyName;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
    }
}
