using Core.Models.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCases.Prints
{
    public class PrintCase
    {
        int availableLines = 10;
        int maxCharPerLine = 16;

        public int EstimateUsedLine(StringBuilder source)
        {
            List<string> lines = new List<string>();

            // tetap makan satu baris untuk print, walaupun dibawah maks karakter per baris
            int usedLine = 1; 

            foreach(var line in source.ToString().Split(' ')) 
            {
                lines.Append(line);
                if (String.Join(' ', lines).Length >= maxCharPerLine)
                {
                    lines.Clear();
                    usedLine++;
                }
                Console.WriteLine(line);
            }

            return usedLine;
        }

        public void EstimateUsedLine(NodePrintModel node)
        {

        }
    }
}
