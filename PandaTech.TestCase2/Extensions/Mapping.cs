using AutoMapper;
using PandaTech.TestCase2.Configuration;

namespace PandaTech.TestCase2.Extensions;

public static class Mapping
{
    public static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<string?, int[][]>()
            .ConvertUsing(source =>
                string.IsNullOrWhiteSpace(source)
                    ? Array.Empty<int[]>()
                    : source
                        .Split(EnvSettings.RowDelimiter, StringSplitOptions.None)
                        .Select(row => row
                            .Split(EnvSettings.ColDelimiter, StringSplitOptions.None)
                            .Select(int.Parse)
                            .ToArray())
                        .ToArray()
            );
    }
}