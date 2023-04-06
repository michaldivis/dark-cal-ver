using DarkCalVer;

namespace ConsoleSampleApp;

internal abstract class Example
{
    public void Run()
    {
        var calVer = CreateVersion();

        Console.WriteLine(Title);
        Console.WriteLine("---");
        Console.WriteLine($"Version string: {calVer.VersionString}");
        Console.WriteLine($"Version: {calVer.Version}");
        Console.WriteLine($"Version number: {calVer.VersionNumber:n0}");
    }

    public abstract string Title { get; }

    protected abstract CalVer CreateVersion();
}

internal class BasicExample : Example
{
    public override string Title => "Basic";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);
        return CalVer.Create(timestamp);
    }
}

internal class PaddingExample : Example
{
    public override string Title => "Preventing leading zeros";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

        return CalVer.Create(new CalVerOptions
        {
            Timestamp = timestamp,
            PreventLeadingZeros = true,
        });
    }
}

internal class CustomReferenceDateExample : Example
{
    public override string Title => "Custom reference date";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

        return CalVer.Create(new CalVerOptions
        {
            Timestamp = timestamp,
            ReferenceDate = new DateTime(2023, 1, 1),
        });
    }
}

internal class AccuracyDaysExample : Example
{
    public override string Title => "Accuracy: days";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

        return CalVer.Create(new CalVerOptions
        {
            Timestamp = timestamp,
            Accuracy = Accuracy.Days
        });
    }
}

internal class AccuracyHoursExample : Example
{
    public override string Title => "Accuracy: hours";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

        return CalVer.Create(new CalVerOptions
        {
            Timestamp = timestamp,
            Accuracy = Accuracy.Hours
        });
    }
}

internal class AccuracyMinutesExample : Example
{
    public override string Title => "Accuracy: minutes";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

        return CalVer.Create(new CalVerOptions
        {
            Timestamp = timestamp,
            Accuracy = Accuracy.Minutes
        });
    }
}

internal class AccuracySecondsExample : Example
{
    public override string Title => "Accuracy: seconds";

    protected override CalVer CreateVersion()
    {
        var timestamp = new DateTime(2023, 1, 1, 08, 15, 30);

        return CalVer.Create(new CalVerOptions
        {
            Timestamp = timestamp,
            Accuracy = Accuracy.Seconds
        });
    }
}