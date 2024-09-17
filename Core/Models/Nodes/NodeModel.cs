using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Nodes
{
    public class NodeModel
    {
        public string Tag;
        public string Text;
        public NodeModel Parent;
        public List<AttrModel> Attributes = new List<AttrModel>();
        public List<NodeModel> Childrens = new List<NodeModel>();
    }
}
