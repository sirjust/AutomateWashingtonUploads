using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmail(IConfiguration config);
    }
}