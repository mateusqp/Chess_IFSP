using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Cell
    {
        public Coordinate cellCoordinate { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string color { get; set; }
        public Cell(int x, int y, string color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            cellCoordinate = new Coordinate(x, y);
        }

    }
}
