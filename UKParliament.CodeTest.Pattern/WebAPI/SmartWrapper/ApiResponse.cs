#region Namespace References
using Newtonsoft.Json;
using System.Collections.Generic;
#endregion

namespace UKParliament.CodeTest.Pattern.WebAPI.SmartWrapper
{
    public class ApiResponse<TResult>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int StatusCode { get; set; }
        public string Message { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsError { get; set; }
        public string ExceptionMessage { get; set; }
        public TResult Result { get; set; }
        public ICollection<TResult> Results { get; set; }
    }
}
