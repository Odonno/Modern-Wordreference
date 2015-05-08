using Microsoft.ApplicationInsights;

namespace Wordreference.Core.Services.Abstract
{
    public interface ITelemetryService
    {
        TelemetryClient TelemetryClient { get; set; }
    }
}
