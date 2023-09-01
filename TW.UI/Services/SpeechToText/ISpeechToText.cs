using System.Globalization;

namespace TW.UI.Services
{
    public interface ISpeechToText
    {
        Task<bool> RequestPermissions();

        Task<string> Listen(CultureInfo culture,
            IProgress<string> recognitionResult, 
            CancellationToken cancellationToken);
    }
}
