using ConsoleSampleApp;

var examples = new Example[]
{
    new BasicExample(),
    new PaddingExample(),
    new CustomReferenceDateExample(),
    new AccuracyHoursExample(),
    new AccuracyMinutesExample(),
    new AccuracySecondsExample()
};

foreach (var example in examples)
{
    example.Run();
    Console.WriteLine();
}