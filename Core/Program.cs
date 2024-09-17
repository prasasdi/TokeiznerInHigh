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
            /*
             * Asumsi mereka ada char eof \0 kalau enggak ada tambahin sendiri
             */
            /// Contoh full string html
            string html = "<div><span>dengan catatan:<span> ini adalah double span</span></span><br/><ul><li>satu. dua dengan <i>italic <span>ini bisa jadi <b>bold</b></span></i></li><li>tiga dan empat dengan <b>bold</b></li></ul><p><div><span>maka dari itu</span> dengan ini saya sampaikan </div>kesimpulannya</p></div>";
            string html1 = "<div ><span >dengan catatan:<span > ini adalah double span</span></span><br /><ul ><li >satu. dua dengan <i >italic <span >ini bisa jadi <b >bold</b></span></i></li><li >tiga dan empat dengan <b >bold</b></li></ul><p ><div ><span >maka dari itu</span> dengan ini saya sampaikan </div>kesimpulannya</p></div>\0";
            
            // Contoh dengan attr
            string bar = "<div class=\"ini\" id=\"parent\" disabled>kan <div id=\"child\"> dimana</div></div>\0";
            string bar1 = "<div class=\"ini\">kan <div > dimana</div ></div >\0";

            string foo = "<div >kakaa</div>\0";
            string foo1 = "<div id=\"div1\">aaaaaa</div><div id=\"div2\">aaaaaa</div>\0";

            string pattern = @"<(/?)([a-zA-Z][a-zA-Z0-9]*)\b[^>]*>|>([^<]*)<|(\s+)";

            Scanner scanner = Scanner.InitScanner(html);

            Token token;

            // container dari nodes
            List<NodeModel> nodes = new List<NodeModel>();

            // pointer ke node N
            NodeModel node = new NodeModel();

            // pointer attr untuk node N
            AttrModel nodeAttr = new AttrModel();
            Console.WriteLine($"Strleng = {scanner.Length}");
            while (true)
            {
                token = ScannerExtension.ScanToken(scanner);
                // print token dimari
                Console.WriteLine($"{token.Type} : {token.Value}");

                switch (token.Type)
                { 
                    case TokenTypeEnum.TOKEN_TAG_START:
                        // kalau tag kosong, asumsikan node adalah sebuah akar
                        if (node.Tag == null)
                        {
                            node = new NodeModel()
                            { 
                                Tag = token.Value,
                            };

                        }
                        else
                        {
                            // buat tempNode untuk dijadikan sebagai anak node
                            var tempNode = new NodeModel()
                            {
                                Tag = token.Value,
                                Parent = node
                            };
                            node.Childrens.Add(tempNode);

                            // arahkan 'pointer' ke anak
                            node = tempNode;
                        }
                        break;
                    case TokenTypeEnum.TOKEN_TEXT:
                        node.Text = token.Value;
                        break;
                    case TokenTypeEnum.TOKEN_ATTR_NAME:
                        nodeAttr = new AttrModel()
                        {
                            Name = token.Value,
                            Node = node
                        };
                        node.Attributes.Add(nodeAttr);
                        // clear node attribute untuk attr selanjutnya
                        nodeAttr = new AttrModel();
                        break;
                    case TokenTypeEnum.TOKEN_ATTR_VALUE:
                        node.Attributes[node.Attributes.Count - 1].Value = token.Value;
                        break;
                    case TokenTypeEnum.TOKEN_SELF_CLOSING:
                        // tambahkan sebagai child dari node
                        node.Childrens.Add(new NodeModel()
                        {
                            Tag = token.Value,
                            Parent = node
                        });
                        break;
                    case TokenTypeEnum.TOKEN_TAG_END:
                        if (node.Parent != null)
                        {
                            node = node.Parent;
                        }
                        else
                        {
                            nodes.Add(node);
                            node = new NodeModel();
                        }
                        break;
                }

                if (token.Type == TokenTypeEnum.TOKEN_EOF)
                {
                    break;
                }
            }

            foreach(var _node in nodes)
            {
                Console.WriteLine($"child node count: {_node.Childrens.Count}");
                foreach (var child in _node.Childrens)
                {
                    Console.WriteLine($"node child: {child.Tag} , parent node: {child.Parent.Tag}");
                }
            }
            scanner.FreeScanner();
            Console.WriteLine();
            Console.WriteLine(JsonSerializer.Serialize(nodes));
            watch.Stop();
            Console.WriteLine();
            Console.WriteLine($"elapsed {watch.ElapsedMilliseconds} ms");
        }
    }
}
