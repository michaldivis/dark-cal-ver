namespace DarkCalVer;

public class CalVerTests
{
    [Fact]
    public void Constructor_Default_DoesntThrow()
    {
        var act = () => CalVer.Create();
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_DefaultOptions_DoesntThrow()
    {
        var act = () => CalVer.Create(new CalVerOptions());
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_CustomValidTimestamp_DoesntThrow()
    {
        var options = new CalVerOptions
        {
            Timestamp = new DateTime(2023, 1, 1),
        };

        var act = () => CalVer.Create(options);

        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_TimestampLessThanReferenceDate_Throws()
    {
        var options = new CalVerOptions
        {
            ReferenceDate = new DateTime(2020, 1, 1),
            Timestamp = new DateTime(2000, 1, 1),
        };

        var act = () => CalVer.Create(options);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GetVersionString_Works(Scenario scenario)
    {
        var options = new CalVerOptions
        {
            ReferenceDate = scenario.ReferenceDate,
            Timestamp = scenario.Timestamp,
            Accuracy = scenario.Accuracy,
            PreventLeadingZeros = scenario.PreventLeadingZeros
        };

        var calVer = CalVer.Create(options);

        calVer.VersionString.Should().Be(scenario.ExpectedVersionString);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GetVersion_Works(Scenario scenario)
    {
        var options = new CalVerOptions
        {
            ReferenceDate = scenario.ReferenceDate,
            Timestamp = scenario.Timestamp,
            Accuracy = scenario.Accuracy,
            PreventLeadingZeros = scenario.PreventLeadingZeros
        };

        var calVer = CalVer.Create(options);

        calVer.Version.Should().Be(scenario.ExpectedVersion);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GetVersionNumber_Works(Scenario scenario)
    {
        var options = new CalVerOptions
        {
            ReferenceDate = scenario.ReferenceDate,
            Timestamp = scenario.Timestamp,
            Accuracy = scenario.Accuracy,
            PreventLeadingZeros = scenario.PreventLeadingZeros
        };

        var calVer = CalVer.Create(options);

        calVer.VersionNumber.Should().Be(scenario.ExpectedVersionNumber);
    }

    public static IEnumerable<object[]> Data =>
        new List<Scenario>()
        {
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Days, false, "25.12.24", 2184),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Days, true, "25.62.74", 2184),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Hours, false, "25.12.2419", 52435),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Hours, true, "25.62.7419", 52435),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Minutes, false, "25.12.241905", 3146105),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Minutes, true, "25.62.741905", 3146105),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Seconds, false, "25.12.24190538", 188766338),
            new Scenario(new DateTime(2025, 12, 24, 19, 05, 38), new DateTime(2020, 1, 1), Accuracy.Seconds, true, "25.62.74190538", 188766338),
        }
        .Select(x => new object[] { x })
        .ToArray();

    public record Scenario(DateTime Timestamp, DateTime ReferenceDate, Accuracy Accuracy, bool PreventLeadingZeros, string ExpectedVersionString, long ExpectedVersionNumber)
    {
        public Version ExpectedVersion => Version.Parse(ExpectedVersionString);
    }
}