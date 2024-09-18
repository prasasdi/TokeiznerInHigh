using Core.Helpers.Enums.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models.Prints
{
    public class NodePrintModel
    {
        [JsonPropertyName("sb")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StringBuilder StringBuilder { get; set; }
        [JsonPropertyName("ul")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int UsedLine { get; set; }
        [JsonPropertyName("ctx")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PrintCtxEnums Context { get; set; }
    }
}
