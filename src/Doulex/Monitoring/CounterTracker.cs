using System.Diagnostics;

namespace Doulex.Monitoring;

/// <summary>
/// Counter is used to count the data and the velocity of the count.
/// </summary>
public class CounterTracker
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateIntervalInMs">The latest velocity update interval in ms</param>
    public CounterTracker(int updateIntervalInMs)
    {
        _updateInterval = updateIntervalInMs;
    }


    /// <summary>
    /// latest velocity value update interval (ms)
    /// </summary>
    private readonly int _updateInterval;

    /// <summary>
    /// Recent count
    /// </summary>
    private readonly Stopwatch _recentCountStopwatch = Stopwatch.StartNew();

    /// <summary>
    /// Total count
    /// </summary>
    private readonly Stopwatch _totalCountStopwatch = Stopwatch.StartNew();

    /// <summary>
    /// Recent count used to calculate the velocity
    /// </summary>
    private int _recentCount;

    /// <summary>
    /// Total count
    /// </summary>
    private int _totalCount;

    /// <summary>
    /// Latest count per second
    /// </summary>
    private int _latestCountPerSec;

    /// <summary>
    /// Total sample count
    /// </summary>
    public int Total => _totalCount;

    /// <summary>
    /// The total average velocity
    /// </summary>
    public int AverageVelocity
    {
        get
        {
            var elapsed = (int)_totalCountStopwatch.ElapsedMilliseconds;
            return elapsed == 0 ? 0 : _totalCount * 1000 / elapsed;
        }
    }

    /// <summary>
    /// The latest velocity per second
    /// </summary>
    public int LatestVelocity
    {
        get
        {
            UpdateVelocity();
            return _latestCountPerSec;
        }
    }

    private void UpdateVelocity()
    {
        var elapsed = (int)_recentCountStopwatch.ElapsedMilliseconds;
        if (elapsed <= _updateInterval)
            return;

        // calc
        _latestCountPerSec = elapsed == 0 ? 0 : _recentCount * 1000 / elapsed;

        // reset stat
        _recentCount = 0;
        _recentCountStopwatch.Restart();
    }

    /// <summary>
    /// Add count to the stat
    /// </summary>
    /// <param name="count">Number of the count</param>
    public void Add(int count)
    {
        Interlocked.Add(ref _totalCount,  count);
        Interlocked.Add(ref _recentCount, count);
    }

    /// <summary>
    /// Reset the stat
    /// </summary>
    public void Reset()
    {
        _totalCount  = 0;
        _recentCount = 0;
        _totalCountStopwatch.Restart();
        _recentCountStopwatch.Restart();
        _latestCountPerSec = 0;
    }
}
