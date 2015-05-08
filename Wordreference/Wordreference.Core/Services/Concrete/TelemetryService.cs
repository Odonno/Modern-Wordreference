using Microsoft.ApplicationInsights;
using Wordreference.Core.Services.Abstract;

namespace Wordreference.Core.Services.Concrete
{
    public class TelemetryService : ITelemetryService
    {
        public TelemetryClient TelemetryClient { get; set; }
    }
}
