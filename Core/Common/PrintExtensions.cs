using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class PrintExtensions
    {
        public static int EstimateUsedLine(string source)
        {
            List<string> lines = new List<string>();

            // tetap makan satu baris untuk print, walaupun dibawah maks karakter per baris
            int usedLine = 1;

            foreach (var line in source.ToString().Split(' '))
            {
                lines.Append(line);
                if (String.Join(' ', lines).Length >= 16)
                {
                    lines.Clear();
                    usedLine++;
                }
            }

            return usedLine;
        }

    }
}
