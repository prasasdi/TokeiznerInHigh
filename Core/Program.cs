using Core.Common;
using Core.Helpers.Enums;
using Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            /*
             * Asumsi mereka ada char eof \0 kalau enggak ada tambahin sendiri
             */
            /// Contoh full string html
            string html = "<div><span>dengan catatan:<span> ini adalah double span</span></span><br/><ul><li>satu. dua dengan <i>italic <span>ini bisa jadi <b>bold</b></span></i></li><li>tiga dan empat dengan <b>bold</b></li></ul><p><div><span>maka dari itu</span> dengan ini saya sampaikan </div>kesimpulannya</p></div>";
            string html1 = "<div ><span >dengan catatan:<span > ini adalah double span</span></span><br /><ul ><li >satu. dua dengan <i >italic <span >ini bisa jadi <b >bold</b></span></i></li><li >tiga dan empat dengan <b >bold</b></li></ul><p ><div ><span >maka dari itu</span> dengan ini saya sampaikan </div>kesimpulannya</p></div>\0";
            
            // Contoh dengan attr
            string bar = "<div class=\"ini\">kan <div> dimana</div></div>\0";
            string bar1 = "<div class=\"ini\">kan <div > dimana</div ></div >\0";

            string foo = "<div >kakaa</div>\0";
            string foo1 = "<div>aaaaaa</div>\0";

            string pattern = @"<(/?)([a-zA-Z][a-zA-Z0-9]*)\b[^>]*>|>([^<]*)<|(\s+)";

            if (!html.Contains('\0'))
            {
                html += '\0';
            }
            Scanner scanner = Scanner.InitScanner(html);

            Token token;
            Console.WriteLine($"Strleng = {scanner.Length}");
            while (true)
            {
                token = ScannerExtension.ScanToken(scanner);
                // print token dimari
                Console.WriteLine($"{token.Type} : {token.Value}");

                if (token.Type == TokenTypeEnum.TOKEN_EOF)
                {
                    break;
                }
            }

            scanner.FreeScanner();
            watch.Stop();
            Console.WriteLine();
            Console.WriteLine($"elapsed {watch.ElapsedMilliseconds} ms");
        }
    }
}
