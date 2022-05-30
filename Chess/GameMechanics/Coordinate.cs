using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Coordinate
    {
        public sbyte x { get; set; }
        public sbyte y { get; set; }

        public Coordinate(sbyte x, sbyte y)
        {
            this.x = x;
            this.y = y;
        }
        public Coordinate(int x, int y)
        {
            this.x = (sbyte)x;
            this.y = (sbyte)y;
        }

        public Coordinate()
        {

        }
    }
}
