using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Helpers.Enums;
using Core.Models;
using Core.Models.Nodes;

namespace Core.Common
{
    public static class ScannerExtension
    {
        public static List<NodeModel> ScanTokens(Scanner scanner)
        {
            Token token;

            // container dari nodes
            List<NodeModel> nodes = new List<NodeModel>();

            // pointer ke node N
            NodeModel node = new NodeModel();

            // pointer attr untuk node N
            AttrModel nodeAttr = new AttrModel();

            while (true)
            {
                token = scanToken(scanner);
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
                            Text = token.Value == "br" ? "\n" : string.Empty,
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
            scanner.FreeScanner();

            return nodes;
        }

        static Token scanToken(Scanner s)
        {
            switch(s.Ctx)
            {
                case CtxEnum.CTX_INITIAL:
                    return init(s);
                case CtxEnum.CTX_IN_TAG:
                    return attribute(s);
                case CtxEnum.CTX_IN_ATTR:
                    return attrValue(s);
                default:
                    return makeErrorToken("Fatal Error: Tokenizer in an invalid state euy");
            }
        }
        static Token init(Scanner s)
        {
            /*
             * scanner->start = scanner->current
             */
            s.Start = s.Current;
            char c = Advance(s);

            switch (c)
            {
                case '<':
                    // as tag
                    return tag(s);
                case '\0':
                    // as make token
                    return makeToken(s, TokenTypeEnum.TOKEN_EOF);
                default:
                    // as text
                    return text(s);
            }
        }
        #region Tag
        static Token tag(Scanner s)
        {
            char c = Peek(s);

            if (c == '/')
            {
                return closingTag(s);
                // closing tag
            }
            else
            {
                // _tag
                return _tag(s);
            }
        }

        static Token _tag(Scanner s)
        {
            Token t;
            s.Start = s.Current;

            while(true)
            {
                char c = Peek(s);

                if (c == ' ')
                {
                    t = makeToken(s, TokenTypeEnum.TOKEN_TAG_START);
                    s.Ctx = CtxEnum.CTX_IN_TAG;
                    break;
                }
                else if (c == '/')
                {
                    t = makeToken(s, TokenTypeEnum.TOKEN_SELF_CLOSING);

                    // skip the / and >
                    Advance(s);
                    Advance(s);

                    s.Ctx = CtxEnum.CTX_INITIAL;
                    break;
                }
                else if (c == '>')
                {
                    t = makeToken(s, TokenTypeEnum.TOKEN_TAG_START);

                    // skip >
                    Advance(s);
                    s.Ctx = CtxEnum.CTX_INITIAL;
                    break;
                }
                else
                {
                    Advance(s);
                }
            }
            return t;
        }
        #endregion
        #region Attribute

        static Token attribute (Scanner s)
        {
            Advance(s);
            s.Start = s.Current;
            Token t;

            while (true)
            {
                char c = Peek(s);

                if (c == ' ')
                {
                    s.Ctx = CtxEnum.CTX_IN_TAG;
                    t = makeToken(s, TokenTypeEnum.TOKEN_ATTR_NAME);
                    break;
                }
                else if(c == '>')
                {
                    t = makeToken(s, TokenTypeEnum.TOKEN_ATTR_NAME);
                    Advance(s);
                    s.Ctx = CtxEnum.CTX_INITIAL;
                    break;
                }
                else if(c == '/')
                {
                    t = makeToken(s, TokenTypeEnum.TOKEN_ATTR_NAME);
                    Advance(s);
                    Advance(s);
                    s.Ctx = CtxEnum.CTX_INITIAL;
                    break;
                }
                else if (c == '=')
                {
                    t = makeToken(s, TokenTypeEnum.TOKEN_ATTR_NAME);
                    s.Ctx = CtxEnum.CTX_IN_ATTR;
                    break;
                }
                else
                {
                    Advance(s);
                }
            }
            return t;
        }

        static Token attrValue(Scanner s)
        {
            // skip = dan "
            Advance(s);
            Advance(s);
            s.Start = s.Current;

            while(true)
            {
                char c = Peek(s);

                // kalau masih lolos juga =
                if (c == '"')
                {
                    break;
                }
                else
                {
                    Advance(s);
                }
            }

            Token t = makeToken(s, TokenTypeEnum.TOKEN_ATTR_VALUE);
            Advance(s);

            char _c = Peek(s);

            if (_c == ' ') 
            {
                s.Ctx = CtxEnum.CTX_IN_TAG;
            }
            else if (_c == '>')
            {
                Advance(s);
                s.Ctx = CtxEnum.CTX_INITIAL;
            }
            else if (_c == '/')
            {
                Advance(s);
                Advance(s);
                s.Ctx = CtxEnum.CTX_INITIAL;
            }
            return t;
        }

        #endregion

        static Token text(Scanner s)
        {
            while (true)
            {
                char c = Peek(s);

                if (c == '<')
                {
                    break;
                }
                else
                {
                    Advance(s);
                }
            }

            return makeToken(s, TokenTypeEnum.TOKEN_TEXT);
        }

        static Token closingTag(Scanner s)
        {
            while(true)
            {
                char c = Advance(s);
                if (c == '>')
                {
                    s.Ctx = CtxEnum.CTX_INITIAL;
                    return makeToken(s, TokenTypeEnum.TOKEN_TAG_END);
                }
            }
        }

        #region Helper
        static char Advance(Scanner s)
        {
            s.Current++;
            return s.Source[s.Current - 1];
        }

        static char Peek(Scanner s)
        {
            return s.Source[s.Current];
        }

        static Token makeToken(Scanner s, TokenTypeEnum type)
        {
            Token t = new Token();
            t.Type = type;
            t.Start = s.Start;
            t.Length = (s.Current - s.Start);
            t.Value = new StringBuilder().Append(s.Source, s.Start, t.Length).ToString();
            return t;
        }

        static Token makeErrorToken(string message)
        {
            Token t = new Token();
            t.Type = TokenTypeEnum.TOKEN_ERROR;
            t.Value = message;
            t.Length = message.Length;
            return t;
        }
        #endregion
    }
}
