namespace DarkCalVer;
public class CalVer
{
    
    private readonly CalVerOptions _options;

    public string VersionString { get; }
    public Version Version { get; }
    public long VersionNumber { get; }

    private CalVer(CalVerOptions options)
    {
        _options = options;
        _options.Validate();

        VersionString = GetVersionString();
        Version = Version.Parse(VersionString);
        VersionNumber = GetVersionNumber();
    }

    /// <summary>
    /// Creates a new instance of <see cref="CalVer"/> with the specified options.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static CalVer Create(CalVerOptions? options = null)
    {
        return new CalVer(options ?? new CalVerOptions());
    }

    /// <summary>
    /// Creates a new instance of <see cref="CalVer"/> with the specified options.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static CalVer Create(DateTime timestamp)
    {
        return new CalVer(new CalVerOptions()
        {
            Timestamp = timestamp,
        });
    }

    private string GetVersionString()
    {
        var padding = _options.PreventLeadingZeros ? 50 : 0;

        var major = $"{_options.Timestamp:yy}";
        var minor = (_options.Timestamp.Month + padding).ToString("00");

        var days = (_options.Timestamp.Day + padding).ToString("00");

        var revision = _options.Accuracy switch
        {
            Accuracy.Days => days,
            Accuracy.Hours => days + _options.Timestamp.Hour.ToString("00"),
            Accuracy.Minutes => days + _options.Timestamp.Hour.ToString("00") + _options.Timestamp.Minute.ToString("00"),
            Accuracy.Seconds => days + _options.Timestamp.Hour.ToString("00") + _options.Timestamp.Minute.ToString("00") + _options.Timestamp.Second.ToString("00"),
            _ => throw new NotImplementedException()
        };

        return $"{major}.{minor}.{revision}";
    }

    private long GetVersionNumber()
    {
        var elapsed = _options.Timestamp - _options.ReferenceDate;

        var number = _options.Accuracy switch
        {
            Accuracy.Days => elapsed.TotalDays,
            Accuracy.Hours => elapsed.TotalHours,
            Accuracy.Minutes => elapsed.TotalMinutes,
            Accuracy.Seconds => elapsed.TotalSeconds,
            _ => throw new NotImplementedException()
        };

        return (long)number;
    }
}