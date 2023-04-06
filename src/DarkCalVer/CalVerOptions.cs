namespace DarkCalVer;

public class CalVerOptions
{
    /// <summary>
    /// The timestamp that will be used to generate the version.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// The a reference date that will be used to generate the <see cref="CalVer.VersionNumber"/>. The version number will the a form a time ellapsed between the <see cref="ReferenceDate"/> and the <see cref="Timestamp"/>, depending on what <see cref="Accuracy"/> value is selected.
    /// </summary>
    public DateTime ReferenceDate { get; init; } = new DateTime(2020, 1, 1);

    /// <summary>
    /// Accuracy of the version. Whatever value is set, the resulting version will be unique on that level. For example, if the <see cref="Accuracy"/> is set to <see cref="Accuracy.Hours"/>, the resulting version will be unique per every hour.
    /// </summary>
    public Accuracy Accuracy { get; init; } = Accuracy.Hours;

    /// <summary>
    /// Will prevent leading zeros by adding 50 to the minor and revision components of the version.
    /// </summary>
    public bool PreventLeadingZeros { get; init; } = false;

    /// <summary>
    /// Validates the options
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Validate()
    {
        if (Timestamp < ReferenceDate)
        {
            throw new ArgumentException($"{nameof(Timestamp)} must be greater than or equal to {nameof(ReferenceDate)}");
        }
    }
}
