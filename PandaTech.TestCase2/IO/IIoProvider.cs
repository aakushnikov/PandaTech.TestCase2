namespace PandaTech.TestCase2.IO;

public interface IIoProvider<out TInput, in TOutput>
{
    TInput GetInput();
    void SetOutput(TOutput data);
    bool IsStopped { get; }
}