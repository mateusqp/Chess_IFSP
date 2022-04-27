using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Square
    {
        public sbyte x;
        public sbyte y;

        public Square(sbyte x, sbyte y)
        {
            this.x = x;
            this.y = y;
        }
        public Square(int x, int y)
        {
            this.x = (sbyte)x;
            this.y = (sbyte)y;
        }
    }
}
