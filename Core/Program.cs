using Core.Common;
using Core.Helpers.Enums;
using Core.Models;
using Core.Models.Nodes;
using Core.UseCases.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Core
{
    internal class Program
    {
        static void Main(string[] args)
        {                
            Stopwatch watch = new Stopwatch();
            watch.Start();

            List<NodeModel> nodes =
                ScannerExtension.ScanTokens
                (
                    Scanner.InitScanner("<div>contoh dari satu tag div</div>")
                );

            //foreach(var node in nodes) 
            //{
            //    var printModel = NodeExtensions.GetInnerText(node);
            //    Console.WriteLine();
            //    Console.WriteLine(printModel.StringBuilder.ToString());
            //}

            #region Examples

            #region inner text
            /*
             * To get and print innerText or innerContent of a node
             */
            Console.WriteLine("get parent #text");
            var pm = NodeExtensions.GetInnerText(nodes[0]);
            Console.WriteLine(pm.StringBuilder);
            Console.WriteLine();

            //
            /*
             * To get and print each innerContent or innerText of children node
             */
            //Console.WriteLine("get childs #text");
            //var pmChilds = NodeExtensions.GetChildrenInnerText(nodes[1]);
            //int ix = 1;
            //foreach (var pmChild in pmChilds)
            //{
            //    Console.WriteLine($"{ix}. {pmChild.StringBuilder}");
            //    Console.WriteLine($"usedLine: {pmChild.UsedLine}");
            //    ix++;
            //}
            //Console.WriteLine();
            #endregion

            #region innerhtml
            //
            /*
             * To get and print innerHTML of an tag
             * 
             * Contohnya dibawah kalau ada lebih dari satu node, tinggal sesuaikan lagi
             */
            var innerHtml = NodeExtensions.GetInnerHTML(nodes[0]);
            Console.WriteLine(innerHtml.StringBuilder);
            Console.WriteLine();

            //
            /*
             * 
             */
            //foreach (var node in nodes)
            //{
            //    var childInnerHtml = NodeExtensions.GetInnerHTML(node);
            //    Console.WriteLine(childInnerHtml.StringBuilder);
            //    Console.WriteLine();
            //}
            //Console.WriteLine();
            #endregion

            #region determiner(ondevelopment)

            //int ava = 5;
            //var n = PrintExtensions.StartDetermine(nodes[1], ref ava);
            //var m = NodeExtensions.GetInnerHTML(n);
            //Console.WriteLine(m.StringBuilder);
            #endregion

            #endregion


            // Karna serializer ini jadi bisa makan 2x lipat waktu eksekusinya, enable kalau mau debug aja
            //Console.WriteLine(JsonSerializer.Serialize(nodes));

            watch.Stop();
            Console.WriteLine($"elapsed {watch.ElapsedMilliseconds} ms");
        }
    }
}
