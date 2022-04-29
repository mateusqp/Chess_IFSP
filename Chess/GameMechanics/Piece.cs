using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Piece
    {
        public Coordinate pos; //position on board
        public bool team; //false = black ; true = White
        public char type; //p = pawn, r = rook; n = knight; b = bishop; q = queen; k = king
        public bool possiblePassant; //false = impossible ; true = possible
        public bool hasMoved; //For castling
        public bool exists;

        public Piece(char type, bool team, Coordinate pos)
        {
            this.pos = pos;
            this.team = team;
            this.type = type;
            possiblePassant = false;
            hasMoved = false;
            exists = true;
        }
    }
}
