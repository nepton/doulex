using Doulex.Performance;

namespace Doulex.IO;

/// <summary>
/// Monitoring stream counter
/// </summary>
public class MonitoringStreamCounter
{
    /// <summary>
    /// The stats of the stream for data sent
    /// </summary>
    public Counter Sent { get; } = new(1000);

    /// <summary>
    /// The stats of the stream for data received
    /// </summary>
    public Counter Received { get; } = new(1000);

    /// <summary>
    /// Reset the stats
    /// </summary>
    public void Reset()
    {
        Sent.Reset();
        Received.Reset();
    }
}
