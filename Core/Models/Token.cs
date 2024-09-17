using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Helpers.Enums;

namespace Core.Models
{
    public class Token
    {
        public TokenTypeEnum Type;
        public string Value;
        public int Start;
        public int Length;
    }
}
