using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Piece
    {
        public Coordinate pos { get; set; } //position on board
        public bool team { get; set; } //false = black ; true = White
        public char type { get; set; } //p = pawn, r = rook; n = knight; b = bishop; q = queen; k = king
        public bool possiblePassant { get; set; } //false = impossible ; true = possible
        public bool hasMoved { get; set; } //For castling
        public bool exists { get; set; }
        public bool isClicked { get; set; }

        public Piece(char type, bool team, Coordinate pos)
        {
            this.pos = pos;
            this.team = team;
            this.type = type;
            possiblePassant = false;
            hasMoved = false;
            exists = true;
            isClicked = false;
        }

        public Piece(char type, bool team, Coordinate pos, bool possibleEnPassant, bool hasMoved)
        {
            this.pos = pos;
            this.team = team;
            this.type = type;
            this.possiblePassant = possibleEnPassant;
            this.hasMoved = hasMoved;
            exists = true;
            isClicked = false;
        }

        public Piece()
        { }
    }
}
