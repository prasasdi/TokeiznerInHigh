using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.Helpers.Enums;
using Core.Models;

namespace Core.Common
{
    public static class ScannerExtension
    {
        public static Token ScanToken(Scanner s)
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
                    if (SelfClosingTagEnums.Enums.Contains(t.Value))
                    {
                        t.Type = TokenTypeEnum.TOKEN_SELF_CLOSING;
                    }
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
