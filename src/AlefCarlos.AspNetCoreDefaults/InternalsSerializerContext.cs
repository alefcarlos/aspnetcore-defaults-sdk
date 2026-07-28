using Microsoft.Extensions.AmbientMetadata;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.Hosting;

[JsonSerializable(typeof(ApplicationMetadata))]
[JsonSerializable(typeof(AppInformation))]
internal partial class InternalsSerializerContext : JsonSerializerContext
{

}