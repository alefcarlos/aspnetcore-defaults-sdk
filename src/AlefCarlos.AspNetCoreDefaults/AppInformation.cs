namespace Microsoft.Extensions.Hosting;

internal record AppInformation(string ApplicationName, string EnvironmentName, IReadOnlyDictionary<string, string?> RuntimeConfigurations);
