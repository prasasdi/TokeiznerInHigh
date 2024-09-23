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
        public static NodeModel StartDetermine(NodeModel node, ref int availableLines)
        {
            if (node.Childrens.Any())
            {
                for (int current = 0; current < node.Childrens.Count && availableLines > 0; current++)
                {
                    Console.WriteLine($"Sisa baris {availableLines}");
                    if (SelfClosingTagEnums.Enums.Contains(node.Childrens[current].Tag))
                    {
                        availableLines--;
                    }
                    else
                    {
                        if (node.Childrens[current].Tag == "#text")
                        {
                            var innerTexts = NodeExtensions.GetInnerText(node.Childrens[current]);
                            var output = calculatePrintedString(innerTexts, ref availableLines);

                            Console.WriteLine(output.ToString());

                            if (innerTexts.StringBuilder.Length != output.Length)
                            {
                                Console.WriteLine($"{innerTexts.StringBuilder.Length} || {output.Length}");
                                node.Childrens[current].Text = output.ToString();
                            }

                            availableLines--;
                        }
                        else
                        {
                            StartDetermine(node.Childrens[current], ref availableLines);
                        }
                    }
                }
            }
            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">as StringBuilder</param>
        /// <param name="availableLines"></param>
        /// <returns>Inner string of html</returns>
        static StringBuilder calculatePrintedString(NodePrintModel source, ref int availableLines)
        {
            StringBuilder sb = new StringBuilder();
            // max karakter = 16

            // point ke awal kata dari suatu baris
            int start = 0;

            for (int current = 0; current < source.StringBuilder.Length && availableLines > 0; current++)
            {
                if (source.StringBuilder[current] == ' ')
                {
                    if (current - start > 16)
                    {
                        availableLines--;
                        // mundur satu char
                        current--;
                        while (source.StringBuilder[current] != ' ')
                        {
                            sb.Remove(sb.Length-1, 1);
                            current--;
                        }
                        sb.Remove(sb.Length - 1, 1);
                        start = current;
                        //sb.Append('|');
                    }
                }
                sb.Append(source.StringBuilder[current]);
            }
            return sb;
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
