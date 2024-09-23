using Core.Helpers.Enums;
using Core.Models.Nodes;
using Core.Models.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class PrintExtensions
    {
        /// <summary>
        /// Determine 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="availableLines"></param>
        /// <returns>String of HTML</returns>
        public static string StartDetermine(NodeModel node, int availableLines = 5)
        {
            if (node.Childrens.Any())
            {
                for (int current = 0; current < node.Childrens.Count && availableLines > 0; current++)
                {
                    Console.WriteLine($"current position {current}");
                    if (SelfClosingTagEnums.Enums.Contains(node.Childrens[current].Tag))
                    {
                        availableLines--;
                    }
                    else
                    {
                        var o = NodeExtensions.GetInnerText(node.Childrens[current]);
                        Console.WriteLine(calculatePrintedString(o, ref availableLines));
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">as StringBuilder</param>
        /// <param name="availableLines"></param>
        /// <returns>Inner string of html</returns>
        static string calculatePrintedString(NodePrintModel source, ref int availableLines)
        {
            StringBuilder sb = new StringBuilder();
            // max karakter = 16

            // point ke awal kata dari suatu baris
            int start = 0;

            for (int current = 0; current < source.StringBuilder.Length && availableLines > 0; current++)
            {
                sb.Append(source.StringBuilder[current]);
                if (source.StringBuilder[current] == ' ')
                {
                    if (current - start > 16)
                    {
                        availableLines--;
                        start = current;
                        sb.Append(availableLines);
                    }
                }
            }
            return sb.ToString();
        }

        static int getStringLength(StringBuilder str, int current)
        {
            int strLeng = 0;
            while(true)
            {
                if (str[current] == ' ')
                {
                    break;
                }
                strLeng++;
            }
            return strLeng;
        }
        
    }
}
