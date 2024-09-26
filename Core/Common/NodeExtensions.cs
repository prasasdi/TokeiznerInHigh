using Core.Helpers.Enums;
using Core.Models.Nodes;
using Core.Models.Prints;
using System;
using System.Linq;
using System.Text;
using Core.Helpers.Enums.Prints;
using Core.UseCases.Prints;
using System.Collections.Generic;

namespace Core.Common
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Get inner HTML
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodePrintModel GetInnerHTML(NodeModel node, ParserEnum Mode = ParserEnum.DEFAULT)
        {
            StringBuilder sb = new StringBuilder();

            
            // construct awal tag
            sb.Append('<');
            sb.Append(node.Tag);

            // construct attributes
            if (node.Attributes.Any())
            {
                sb.Append(' ');
                foreach (var attr in node.Attributes)
                { 
                    sb.Append(attr.Name);
                    sb.Append('=');
                    sb.Append('"');
                    sb.Append(attr.Value);
                    sb.Append('"');

                    // tambah ' ' kalau belum diakhir node
                    if (!attr.Equals(node.Attributes.Last()))
                    {
                        sb.Append(' ');
                    }
                }
            }

            // close an opening tag
            sb.Append('>');
            
            // construct member(s) of the tag
            if (node.Childrens.Any())
            {
                foreach(var child in node.Childrens)
                {
                    switch(Mode)
                    {
                        case ParserEnum.PRINT_GO:
                            if (child.Type != NodeTypeEnums.DT_CTX_HOLDPRINT || child.Type == NodeTypeEnums.DT_CTX_READ)
                            {
                                if (child.Tag != "#text")
                                {
                                    var c = GetInnerHTML(child, Mode);
                                    sb.Append(c.StringBuilder);
                                }
                                else
                                {
                                    sb.Append(child.Text);
                                }
                            }
                            break;
                        case ParserEnum.PRINT_HOLD:
                            if (child.Type == NodeTypeEnums.DT_CTX_HOLDPRINT || child.Type == NodeTypeEnums.DT_CTX_READ)
                            {
                                if (child.Tag != "#text")
                                {
                                    var c = GetInnerHTML(child, Mode);
                                    sb.Append(c.StringBuilder);
                                }
                                else
                                {
                                    sb.Append(child.Text);
                                }
                            }
                            break;
                        case ParserEnum.DEFAULT:
                            if (child.Tag != "#text")
                            {
                                var c = GetInnerHTML(child, Mode);
                                sb.Append(c.StringBuilder);
                            }
                            else
                            {
                                sb.Append(child.Text);
                            }
                            break;

                    }
                        
                }
            }

            // construct closing tag
            // check apakah tag ini berupa self-closing tag
            if (!SelfClosingTagEnums.Enums.Contains(node.Tag))
            {
                sb.Append('<');
                sb.Append('/');
                sb.Append(node.Tag);
                sb.Append('>');
            }
            return new NodePrintModel()
            {
                StringBuilder = sb,
                UsedLine = 1,
                Context = PrintCtxEnums.PRINT_CTX_INNERHTML
            };
        }

        /// <summary>
        /// Get innerText of each children
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Return list of children of a node print model</returns>
        public static List<NodePrintModel> GetChildrenInnerText(NodeModel node)
        {
            List<NodePrintModel> lnpm = new List<NodePrintModel>();
            foreach(var childNode in node.Childrens)
            {
                if (!SelfClosingTagEnums.Enums.Contains(childNode.Tag))
                {
                    lnpm.Add(GetInnerText(childNode));
                }
            }
            return lnpm;
        }

        /// <summary>
        /// Get innerText of an node
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Return a node print model</returns>
        public static NodePrintModel GetInnerText(NodeModel node)
        {
            var npm = new NodePrintModel()
            {
                UsedLine = 1,
                Context = node.Tag == "ul" ? PrintCtxEnums.PRINT_CTX_LIST : PrintCtxEnums.PRINT_CTX_DEFAULT
            };
            npm.Context = SelfClosingTagEnums.Enums.Contains(node.Tag) ? PrintCtxEnums.PRINT_CTX_SELFCLOSING : npm.Context;
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
            else
            {
                // asumsi tag adalah #text
                if (node.Tag == "#text")
                {
                    sb.Append(node.Text);
                }
            }
            return sb;
        }


    }
}
