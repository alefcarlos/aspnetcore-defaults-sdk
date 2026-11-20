using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.Hosting;

[JsonSerializable(typeof(ApplicationMetadata))]
[JsonSerializable(typeof(AppInformation))]
[JsonSerializable(typeof(LogLevel))]
internal partial class InternalsSerializerContext : JsonSerializerContext
{

}