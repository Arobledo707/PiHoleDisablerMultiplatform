using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using PiHoleDisablerMultiplatform.Services;
using System.Threading.Tasks;
using ZXing.Mobile;

[assembly: Dependency(typeof(PiHoleDisablerMultiplatform.iOS.Scan.QRScaniOS))]
namespace PiHoleDisablerMultiplatform.iOS.Scan
{
    public class QRScaniOS : IQRScan
    {
        public async Task<string> AsyncScan()
        {
            MobileBarcodeScanningOptions scanOptions = new MobileBarcodeScanningOptions();

            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            var result = await scanner.Scan(scanOptions);

            return result.Text;
        }
    }
}