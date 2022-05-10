using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Cell
    {
        public Coordinate cellCoordinate;
        public int x;
        public int y;
        public string color;
        public Cell(int x, int y, string color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            cellCoordinate = new Coordinate(x, y);
        }

    }
}
