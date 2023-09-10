namespace PandaTech.TestCase2.Services;

public sealed class SeekerService<TValue> : ISeekerService<TValue, int>
{
    public int Seek(IEnumerable<IEnumerable<TValue>> source, TValue value)
    {
        var counter = 0;
        if (value is null)
            return counter;    
        
        Parallel.ForEach(
            source,
            row =>
            {
                Parallel.ForEach(
                    row,
                    col =>
                    {
                        if (!value.Equals(col)) return;
                        Interlocked.Increment(ref counter);
                    });
            }
        );

        return counter;
    }
}