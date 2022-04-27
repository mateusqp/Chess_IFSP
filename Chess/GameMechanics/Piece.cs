using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Piece
    {
        public Square pos; //position on board
        public bool team; //0 = black ; 1 = White
        public char type; //p = pawn, r = rook; n = knight; b = bishop; q = queen; k = king
        public bool possiblePassant; //0 = impossible ; 1 = possible
        public bool hasMoved; //For castling

        public Piece(char type, bool team, Square pos)
        {
            this.pos = pos;
            this.team = team;
            this.type = type;
            possiblePassant = false;
            hasMoved = false;
        }
    }
}
