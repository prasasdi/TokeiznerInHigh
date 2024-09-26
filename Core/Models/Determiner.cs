using Core.Helpers.Enums;
using Core.Models.Nodes;
using Core.Models.Prints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Much higher abstraction above NodeModel in order to determine printed string in html
    /// </summary>
    public class Determiner
    {
        /// <summary>
        /// Menunjukan tingkat atau 'lantai'
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// Banyak Node yang ada dalam context dan lantai tersebut
        /// </summary>
        public List<NodeModel> Nodes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        NodeTypeEnums Ctx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">Keturunan keberapa</param>
        /// <returns></returns>
        public Determiner InitDeterminer(int level, NodeTypeEnums ctx)
        {
            return new Determiner()
            { 
                Ctx = ctx,
                Level = level
            };

        }
    }
}

