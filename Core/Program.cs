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
            
            Scanner scanner = Scanner.InitScanner("<p style=\"text-align: justify\">Pastikan pada mesin granulasi bagian \"<b>Inlet Product dan Pipa / Selang Suction</b><span >\" </span>(Jika Digunakan) : </p><ul><li style=\"text-align: justify\">Tidak terdapat sisa produk sesuai dipersyaratkan dari produk sebelumnya / batch lainnya / produk lainnya </li><li style=\"text-align: justify\">Tidak terdapat debu dan kotoran serta dalam kondisi yang baik untuk digunakan </li><li style=\"text-align: justify\">Tidak terpasang / terhubung dengan benar dan kencang. </li></ul><p style=\"text-align: justify\"><b>Catatan : </b></p><p style=\"text-align: justify\">1. Jika <b>digunakan : </b></p><ul><li style=\"text-align: justify\">pilih <b>Sesuai, Jika hasil memenuhi syarat </b></li><li style=\"text-align: justify\">pilih <b>Tidak Sesuai, jika hasil tidak memenuhi syarat </b></li></ul><p style=\"text-align: justify\">2. Jika <b>tidak digunakan</b>, maka pilih <b>NA</b>. </p>");

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
                        //node.Text = token.Value;
                        node.Childrens.Add(new NodeModel()
                        { 
                            Tag = "#text",
                            Text = token.Value,
                            Parent = node
                        });
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
