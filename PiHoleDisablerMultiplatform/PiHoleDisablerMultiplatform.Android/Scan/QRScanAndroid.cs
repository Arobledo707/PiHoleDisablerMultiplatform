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
using PiHoleDisablerMultiplatform.Services;
using System.Threading.Tasks;
using ZXing.Mobile;

[assembly: Dependency(typeof(PiHoleDisablerMultiplatform.Droid.Scan.QRScanAndroid))]

namespace PiHoleDisablerMultiplatform.Droid.Scan
{
    public class QRScanAndroid : IQRScan
    {
        public QRScanAndroid() 
            : base()
        {
        }
        public async Task<string> AsyncScan()
        {
            MobileBarcodeScanningOptions scanOptions = new MobileBarcodeScanningOptions();

            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            var result = await scanner.Scan(scanOptions);

            return result.Text;
        }
    }
}