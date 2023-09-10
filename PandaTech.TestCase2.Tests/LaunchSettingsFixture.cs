using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PandaTech.TestCase2.Tests;

[SetUpFixture]
public sealed class LaunchSettingsFixture
{
    public LaunchSettingsFixture()
    {
        using var file = File.OpenText("Properties\\launchSettings.json");
        var reader = new JsonTextReader(file);
        var jObject = JObject.Load(reader);

        var variables = jObject
            .GetValue("profiles")
            .First
            .SelectMany(profile => profile.Children<JProperty>())
            .Where(prop => prop.Name == "environmentVariables")
            .SelectMany(prop => prop.Value.Children<JProperty>())
            .ToList();

        Parallel.ForEach(variables,
            variable => Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString()));
    }
}