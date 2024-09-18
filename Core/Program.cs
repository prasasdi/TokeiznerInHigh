using Core.Common;
using Core.Helpers.Enums;
using Core.Models;
using Core.Models.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            List<NodeModel> nodes = ScannerExtension.ScanTokens(Scanner.InitScanner("<div></div>"));

            // Karna serializer ini jadi bisa makan 2x lipat waktu eksekusinya, enable kalau mau debug aja
            //Console.WriteLine(JsonSerializer.Serialize(nodes));

            watch.Stop();
            Console.WriteLine($"elapsed {watch.ElapsedMilliseconds} ms");
            Console.ReadLine();
        }
    }
}
