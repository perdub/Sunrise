namespace Sunrise.Integrations.Vk.Types;

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class LongPollResult
{
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("server")]
    public Uri Server { get; set; }

    [JsonProperty("ts")]
    public string Ts { get; set; }
}