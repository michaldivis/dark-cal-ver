namespace DarkCalVer;

public class CalVerOptionsTests
{
    [Fact]
    public void Validate_Default_Works()
    {
        var act = () => new CalVerOptions().Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_CustomValidTimestamp_Works()
    {
        var options = new CalVerOptions
        {
            Timestamp = new DateTime(2023, 1, 1),
        };

        var act = () => options.Validate();

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_CustomValidReferenceDate_Works()
    {
        var options = new CalVerOptions
        {
            ReferenceDate = new DateTime(2023, 1, 1),
        };

        var act = () => options.Validate();

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_TimestampLessThanReferenceDate_Throws()
    {
        var options = new CalVerOptions
        {
            ReferenceDate = new DateTime(2020, 1, 1),
            Timestamp = new DateTime(2000, 1, 1),
        };

        var act = () => options.Validate();

        act.Should().Throw<ArgumentException>();
    }
}