namespace PandaTech.TestCase2.IO;

public sealed class ConsoleIoProvider : IIoProvider<string?, string?>
{
    private readonly string _openMessage;
    private readonly string _stopWord;
    private readonly string[]? _requirements;

    public ConsoleIoProvider(string openMessage, string stopWord, string[]? requirements = null) =>
        (_openMessage, _stopWord, _requirements) = (openMessage, stopWord, requirements);
    
    public string? GetInput()
    {
        if (_requirements is not null)
        {
            foreach (var line in _requirements)
                Console.WriteLine(line);
            Console.WriteLine(string.Empty.PadLeft(20, '='));
        }

        Console.WriteLine(_openMessage);
        
        var input = Console.ReadLine();
        IsStopped = string.Compare(input, _stopWord, StringComparison.OrdinalIgnoreCase) == 0;
        return IsStopped ? string.Empty : input;
    }

    public void SetOutput(string? data)
    {
        Console.WriteLine(data);
        Console.WriteLine(string.Empty.PadLeft(20, '='));
    }
    
    public bool IsStopped { get; private set; }
}