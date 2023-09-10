namespace PandaTech.TestCase2.Configuration;

using System;

public static class EnvSettings
{
    private static readonly Type[] SupportedTypes =
        { typeof(string), typeof(int), typeof(long), typeof(double), typeof(bool) };
    
    public static readonly string Namespace;

    public static readonly int[] MatrixAllowedValues = { };
    public static readonly string OpenMessage;
    public static readonly string StopWord;
    public static readonly int LookedValue;
    public static readonly int MatrixMaxX;
    public static readonly int MatrixMaxY;
    public static readonly string RowDelimiter;
    public static readonly string ColDelimiter;

    static EnvSettings()
    {
        Namespace = typeof(EnvSettings).Namespace ?? string.Empty;
        
        MatrixAllowedValues = MatrixAllowedValues.InitializeArrayEnvAssert(nameof(MatrixAllowedValues));
        OpenMessage = OpenMessage.InitializeEnvAssert(nameof(OpenMessage));
        StopWord = StopWord.InitializeEnvAssert(nameof(StopWord));
        LookedValue = LookedValue.InitializeEnvAssert(nameof(LookedValue));
        MatrixMaxX = MatrixMaxX.InitializeEnvAssert(nameof(MatrixMaxX));
        MatrixMaxY = MatrixMaxY.InitializeEnvAssert(nameof(MatrixMaxY));
        RowDelimiter = RowDelimiter.InitializeEnvAssert(nameof(RowDelimiter));
        ColDelimiter = ColDelimiter.InitializeEnvAssert(nameof(ColDelimiter));
    }

    private static T InitializeEnvAssert<T>(this T? def, string name, T? defValue = default)
    {
        var val = def.InitializeEnv(name, defValue);

        if (val == null) throw new ArgumentException(name);
        
        if (SupportedTypes.All(t => t != typeof(T)))
            throw new ArgumentOutOfRangeException();

        return val;
    }

    private static T? InitializeEnv<T>(this T? def, string name, T? defValue = default)
    {
        var stringValue = Environment.GetEnvironmentVariable($"{Namespace}.{name}");

        if (string.IsNullOrEmpty(stringValue)) return defValue;

        return (T)Convert.ChangeType(stringValue, typeof(T));
    }

    private static T[] InitializeArrayEnv<T>(this T[] def, string name, T? defValue = default)
    {
        var stringValue = Environment.GetEnvironmentVariable($"{Namespace}.{name}");

        if (string.IsNullOrEmpty(stringValue)) return Array.Empty<T>();

        return stringValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => (T)Convert.ChangeType(v, typeof(T))).ToArray();
    }

    private static T[] InitializeArrayEnvAssert<T>(this T[] def, string name, T? defValue = default)
    {
        var val = InitializeArrayEnv(def, name, defValue);
        
        if (val == null) throw new ArgumentException(name);

        if (!SupportedTypes.Contains(typeof(T)))
            throw new ArgumentOutOfRangeException();

        return val;
    }
}
