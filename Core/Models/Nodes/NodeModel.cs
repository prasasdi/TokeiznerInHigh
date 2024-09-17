using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models.Nodes
{
    public class NodeModel
    {
        [JsonPropertyName("tg")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Tag { get; set; }
        [JsonPropertyName("tx")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Text { get; set; }
        [JsonIgnore]
        public NodeModel Parent { get; set; }
        [JsonPropertyName("atrs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<AttrModel> Attributes { get; set; } = new List<AttrModel>();
        [JsonPropertyName("chlds")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<NodeModel> Childrens { get; set; } = new List<NodeModel>();
    }
}
