using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Cell
    {
        public int row = -1;
        public int column = -1;
        public int n = 0;
        public bool flagged = false;
        public bool touched = false;
        public bool mine = false;
    }
}
