using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Enums
{
    public enum TokenTypeEnum
    {
        TOKEN_TAG_START,
        TOKEN_TAG_END,
        TOKEN_SELF_CLOSING,
        TOKEN_TEXT,
        TOKEN_ATTR_NAME,
        TOKEN_ATTR_VALUE,
        TOKEN_ERROR,
        TOKEN_EOF
    }
}
