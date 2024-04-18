namespace Doulex.Monitoring;

/// <summary>
/// Monitoring stream counter
/// </summary>
public class MonitoringStreamCounter
{
    /// <summary>
    /// The stats of the stream for data sent
    /// </summary>
    public CounterTracker Sent { get; } = new(1000);

    /// <summary>
    /// The stats of the stream for data received
    /// </summary>
    public CounterTracker Received { get; } = new(1000);

    /// <summary>
    /// Reset the stats
    /// </summary>
    public void Reset()
    {
        Sent.Reset();
        Received.Reset();
    }
}
