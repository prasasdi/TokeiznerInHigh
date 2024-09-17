using Core.Common;
using Core.Helpers.Enums;
using Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// Contoh full string html
            string html = "<div><span>dengan catatan:<span> ini adalah double span</span></span><br/><ul><li>satu. dua dengan <i>italic <span>ini bisa jadi <b>bold</b></span></i></li><li>tiga dan empat dengan <b>bold</b></li></ul><p><div><span>maka dari itu</span> dengan ini saya sampaikan </div>kesimpulannya</p></div>\0";
            string html1 = "<div ><span >dengan catatan:<span > ini adalah double span</span></span><br /><ul ><li >satu. dua dengan <i >italic <span >ini bisa jadi <b >bold</b></span></i></li><li >tiga dan empat dengan <b >bold</b></li></ul><p ><div ><span >maka dari itu</span> dengan ini saya sampaikan </div>kesimpulannya</p></div>\0";
            
            // Contoh dengan attr
            string bar = "<div class=\"ini\">kan <div> dimana</div></div>\0";
            string bar1 = "<div class=\"ini\">kan <div > dimana</div ></div >\0";

            string foo = "<div >kakaa</div>\0";
            string foo1 = "<div>aaaaaa</div>\0";

            string pattern = @"<(/?)([a-zA-Z][a-zA-Z0-9]*)\b[^>]*>|>([^<]*)<|(\s+)";
            Scanner scanner = Scanner.InitScanner(bar);

            Token token;
            Console.WriteLine($"Strleng = {scanner.Length}");
            while (true)
            {
                token = ScannerExtension.ScanToken(scanner);
                // print token dimari
                Console.WriteLine($"{token.Type} : {token.Value} {token.Length}");

                if (token.Type == TokenTypeEnum.TOKEN_EOF)
                {
                    break;
                }
            }

            scanner.FreeScanner();
        }
    }
}
