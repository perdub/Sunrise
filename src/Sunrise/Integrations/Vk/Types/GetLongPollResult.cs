namespace Sunrise.Integrations.Vk.Types;

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
public partial class TopLevelLongPollResult{
    [JsonProperty("response")]
    public LongPollResult response{get;set;}
}
public partial class LongPollResult
{
    [JsonProperty("key")]
    public string key { get; set; }

    [JsonProperty("server")]
    public string server { get; set; }

    [JsonProperty("ts")]
    public string ts { get; set; }
}