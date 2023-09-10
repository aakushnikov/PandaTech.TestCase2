namespace PandaTech.TestCase2.Services;

public interface ISeekerService<in TValue, out TResult>
{
    TResult Seek(IEnumerable<IEnumerable<TValue>> source, TValue value);
}