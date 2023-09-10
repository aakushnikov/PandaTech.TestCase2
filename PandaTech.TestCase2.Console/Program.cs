using AutoMapper;
using PandaTech.TestCase2.Configuration;
using PandaTech.TestCase2.Console;
using PandaTech.TestCase2.Extensions;
using PandaTech.TestCase2.IO;
using PandaTech.TestCase2.Services;
using PandaTech.TestCase2.Validators;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Console", EnvSettings.Namespace)
    .WriteTo.Console(
        restrictedToMinimumLevel: LogEventLevel.Debug,
        theme: ConsoleTheme.None
    )
    .CreateLogger();

var mapper = new MapperConfiguration(Mapping.CreateMap).CreateMapper();
var seeker = new SeekerService<int>();
var matrixValidator = new MatrixValidator();
var consoleProvider = new ConsoleIoProvider(
    EnvSettings.OpenMessage,
    EnvSettings.StopWord,
    new[]
    {
        $"Rows must be separated by the \"{EnvSettings.RowDelimiter}\".",
        $"Cols must be separated by the \"{EnvSettings.ColDelimiter}\".",
        $"Allowed value are \"{string.Join(',', EnvSettings.MatrixAllowedValues)}\".",
        $"Max rows: {EnvSettings.MatrixMaxY}. Max cols: {EnvSettings.MatrixMaxX}",
        $"Type \"{EnvSettings.StopWord}\" (non case-sensitive) to abort application"
    }
);

await new Dispatcher(consoleProvider, seeker, mapper, matrixValidator).Run();
