
using System.Threading.Tasks;

namespace PiHoleDisablerMultiplatform.Services
{
    public interface IQRScan
    {
        Task<string> AsyncScan();
    }
}
