using Core.Helpers.Enums;
using Core.Models.Nodes;
using Core.Models.Prints;
using System;
using System.Linq;
using System.Text;
using Core.Helpers.Enums.Prints;

namespace Core.Common
{
    public static class NodeExtensions
    {
        public static NodePrintModel GetInnerText(NodeModel node)
        {
            var npm = new NodePrintModel()
            {
                UsedLine = 1,
                Context = node.Tag == "ul" ? PrintCtxEnums.PRINT_CTX_LIST : PrintCtxEnums.PRINT_CTX_DEFAULT
            };
            npm.StringBuilder = getInnerText(node, npm);
            return npm;
        }
        static StringBuilder getInnerText(NodeModel node, NodePrintModel npm)
        {
            StringBuilder sb = new StringBuilder();
            if (node.Childrens.Any())
            {
                foreach(var child in node.Childrens)
                {
                    switch(child.Tag)
                    {
                        case "#text":
                            sb.Append(child.Text);
                            if (child.Parent.Tag == "ul")
                            {
                                sb.Append('\n');
                                npm.UsedLine++;
                            }
                            break;
                        case "li":
                            sb.Append(getInnerText(child, npm));
                            if (!child.Equals(node.Childrens.Last()))
                            {
                                sb.Append('\n');
                                npm.UsedLine++;
                            }
                            break;
                        default:
                            if (SelfClosingTagEnums.Enums.Contains(child.Tag))
                            {
                                break;
                            };
                            sb.Append(getInnerText(child, npm));
                            break;
                    }
                }
            }
            return sb;
        }


    }
}
