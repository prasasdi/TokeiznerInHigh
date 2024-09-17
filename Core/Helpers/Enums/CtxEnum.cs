using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Enums
{
    public enum CtxEnum
    {
        /// <summary>
        /// Parsing a new tag next
        /// </summary>
        CTX_INITIAL,
        /// <summary>
        /// parsed the opening tag and need to parse attributes next
        /// </summary>
        CTX_IN_TAG,
        /// <summary>
        /// parsed an attribute's name and need to parse its value next
        /// </summary>
        CTX_IN_ATTR,
    }
}
