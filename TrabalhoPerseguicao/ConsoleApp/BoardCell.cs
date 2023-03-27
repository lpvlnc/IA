using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class BoardCell
    {
        /// <summary>
        /// character code 32
        /// </summary>
        public static readonly char FreeCell = ' ';

        /// <summary>
        /// character code 178
        /// </summary>
        public static readonly char BlockedCell = '▓';

        /// <summary>
        /// character code 184
        /// </summary>
        public static readonly char NPCCell = '╕';

        /// <summary>
        /// character code 190
        /// </summary>
        public static readonly char PlayerCell = '╛';
    }
}
