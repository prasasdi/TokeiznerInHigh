﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Core.Helpers.Enums;

namespace Core.Models
{
    public class Scanner
    {
        /// <summary>
        /// Semisalnya ada yang mau dipotong didalamnya, tapi enggak mungkin lah ya
        /// </summary>
        public StringBuilder Source;
        /// <summary>
        /// Pointer awal karakter
        /// </summary>
        public int Start = 0;
        /// <summary>
        /// Pointer pada string yang di scan
        /// </summary>
        public int Current = 0;
        /// <summary>
        /// Asumsi pointer ke node yang mana untuk menjadikan DOM Tree
        /// </summary>
        public int Line { get; set; } = 1;
        /// <summary>
        /// StrLen
        /// </summary>
        public int Length;
        /// <summary>
        /// contex enum to know in what state the tokenizer is currently in
        /// </summary>
        public CtxEnum Ctx { get; set; }

        public static Scanner InitScanner(string Source)
        {
            StringBuilder sb = new StringBuilder(Source);
            if (!Source.Contains('\0'))
            {
                sb.Append('\0');
            }
            return new Scanner()
            {
                Source = sb,
                Length = Source.Length,
                Current = 0,
                Start = 0,
                Ctx = CtxEnum.CTX_INITIAL,
            };
        }

        public void FreeScanner()
        {
            Source.Clear();
            Start = 0;
            Current = 0;
            Length = 0;
            Ctx = CtxEnum.CTX_INITIAL;
        }
    }
}
