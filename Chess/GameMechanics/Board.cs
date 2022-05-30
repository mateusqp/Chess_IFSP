using Force.DeepCloner;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Chess.GameMechanics
{
    public class Board
    {
        public List<Piece> pieces { get; set; }
        public List<Cell> cells { get; set; }
        public List<Rectangle> clickableRects { get; set; }
        public Board(bool team1) //Standard Board
        {
            pieces = new List<Piece>();
            cells = new List<Cell>();
            clickableRects = new List<Rectangle>();

            if (team1 || !team1) //Doesnt matter which team is which, inverting coordinates in game.xaml.cs will do it;
            {
                for (int y = 1; y < 9; y++) //inserting pieces
                {

                    for (int x = 1; x < 9; x++)
                    {
                        char type = '0';
                        switch (x)
                        {
                            case 1:
                                type = 'r';
                                break;
                            case 2:
                                type = 'n';
                                break;
                            case 3:
                                type = 'b';
                                break;
                            case 4:
                                type = 'q';
                                break;
                            case 5:
                                type = 'k';
                                break;
                            case 6:
                                type = 'b';
                                break;
                            case 7:
                                type = 'n';
                                break;
                            case 8:
                                type = 'r';
                                break;

                        }
                        switch (y)
                        {
                            case 8:
                                pieces.Add(new Piece(type, true, new Coordinate((sbyte)x, (sbyte)y)));
                                break;

                            case 1:
                                pieces.Add(new Piece(type, false, new Coordinate((sbyte)x, (sbyte)y)));
                                break;

                            case 7:
                                pieces.Add(new Piece('p', true, new Coordinate((sbyte)x, (sbyte)y)));
                                break;

                            case 2:
                                pieces.Add(new Piece('p', false, new Coordinate((sbyte)x, (sbyte)y)));
                                break;
                        }

                    }
                }
            }
        }

        public static List<Coordinate> PieceMovementVector(Piece p)
        {
            List<Coordinate> c = new();
            switch (p.type)
            {
                case 'r':
                    c.Add(new Coordinate(1, 0));
                    c.Add(new Coordinate(-1, 0));
                    c.Add(new Coordinate(0, 1));
                    c.Add(new Coordinate(0, -1));
                    break;

                case 'n':
                    c.Add(new Coordinate(1, 2));
                    c.Add(new Coordinate(-1, 2));
                    c.Add(new Coordinate(1, -2));
                    c.Add(new Coordinate(-1, -2));
                    c.Add(new Coordinate(2, 1));
                    c.Add(new Coordinate(2, -1));
                    c.Add(new Coordinate(-2, 1));
                    c.Add(new Coordinate(-2, -1));

                    break;

                case 'b':
                    c.Add(new Coordinate(1, 1));
                    c.Add(new Coordinate(-1, 1));
                    c.Add(new Coordinate(1, -1));
                    c.Add(new Coordinate(-1, -1));
                    break;

                case 'q':
                    c.Add(new Coordinate(1, 0));
                    c.Add(new Coordinate(-1, 0));
                    c.Add(new Coordinate(0, 1));
                    c.Add(new Coordinate(0, -1));
                    c.Add(new Coordinate(1, 1));
                    c.Add(new Coordinate(-1, 1));
                    c.Add(new Coordinate(1, -1));
                    c.Add(new Coordinate(-1, -1));
                    break;

                case 'k':
                    c.Add(new Coordinate(1, 0));
                    c.Add(new Coordinate(-1, 0));
                    c.Add(new Coordinate(0, 1));
                    c.Add(new Coordinate(0, -1));
                    c.Add(new Coordinate(1, 1));
                    c.Add(new Coordinate(-1, 1));
                    c.Add(new Coordinate(1, -1));
                    c.Add(new Coordinate(-1, -1));
                    break;

                case 'p':
                    if (p.team) //White
                    {
                        c.Add(new Coordinate(0, -1));
                        c.Add(new Coordinate(0, -2));
                        c.Add(new Coordinate(1, -1));
                        c.Add(new Coordinate(-1, -1));
                    }

                    else
                    {
                        c.Add(new Coordinate(0, 1));
                        c.Add(new Coordinate(0, 2));
                        c.Add(new Coordinate(1, 1));
                        c.Add(new Coordinate(-1, 1));
                    }
                    break;

            }
            return c;
        }
        public List<Coordinate> MovementPossibilities(Piece p, List<Piece> pieces)
        {
            List<Coordinate> cF = new(); //Final list

            foreach (Coordinate c in PieceMovementVector(p))
            {
                Coordinate newCoord = new();

                for (int i = 1; i < 8; i++)
                {

                    newCoord = SumCoordinate(p.pos, MultiplyCoordinate(c, i));


                    if (p.type == 'p')
                        break;
                    if (p.type == 'n')
                        break;
                    if (p.type == 'k')
                        break;

                    if (newCoord.x > 0 && newCoord.x < 9 && newCoord.y > 0 && newCoord.y < 9 && p.type != 'p' && p.type != 'n' && p.type != 'k')
                    {
                        foreach (Piece p2 in pieces)
                        {
                            if (p.team == p2.team && newCoord.x == p2.pos.x && newCoord.y == p2.pos.y)
                            {
                                i = 9;
                                break;
                            }
                            if (p.team != p2.team && newCoord.x == p2.pos.x && newCoord.y == p2.pos.y)
                            {
                                cF.Add(newCoord);
                                i = 9;
                                break;
                            }
                        }
                        if (i < 9)
                        {
                            cF.Add(newCoord);
                        }
                    }

                }

                bool possible;

                newCoord = SumCoordinate(p.pos, c);

                if (newCoord.x > 0 && newCoord.x < 9 && newCoord.y > 0 && newCoord.y < 9)
                {
                    switch (p.type)
                    {
                        case 'p':
                            possible = true;
                            foreach (Piece p2 in pieces)
                            {
                                if (Math.Abs(c.y) == 2 && p.hasMoved)
                                {
                                    possible = false;
                                    break;
                                }

                                if (c.y == 2)
                                {
                                    if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y)
                                    {
                                        possible = false;
                                        break;
                                    }
                                    if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y - 1)
                                    {
                                        possible = false;
                                        break;
                                    }
                                }

                                if (c.y == -2)
                                {
                                    if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y)
                                    {
                                        possible = false;
                                        break;
                                    }
                                    if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y + 1)
                                    {
                                        possible = false;
                                        break;
                                    }
                                }
                            }

                            if (c.y == 1 || c.y == -1)
                            {
                                if (c.x == 0)
                                {
                                    foreach (Piece p2 in pieces)
                                    {
                                        if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y)
                                        {
                                            possible = false;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (c.y == 1 || c.y == -1)
                            {
                                if (c.x != 0)
                                {
                                    possible = false;
                                    foreach (Piece p2 in pieces)
                                    {
                                        if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y && p.team != p2.team)
                                        {
                                            possible = true;
                                            break;
                                        }
                                    }
                                    foreach (Piece p2 in pieces)
                                    {
                                        if (c.x == 1)
                                        {
                                            if (p.pos.y == p2.pos.y && p.pos.x == p2.pos.x - 1 && p2.possiblePassant)
                                            {
                                                possible = true;
                                                break;
                                            }
                                        }
                                        if (c.x == -1)
                                        {
                                            if (p.pos.y == p2.pos.y && p.pos.x == p2.pos.x + 1 && p2.possiblePassant)
                                            {
                                                possible = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                            if (possible)
                            {
                                cF.Add(newCoord);
                            }
                            break;

                        case 'k':

                            possible = true;

                            foreach (Piece p2 in pieces)
                            {
                                if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y && p2.team == p.team)
                                {
                                    possible = false;
                                }
                            }
                            if (possible)
                            {
                                cF.Add(newCoord);
                            }
                            break;

                        case 'n':

                            possible = true;

                            foreach (Piece p2 in pieces)
                            {
                                if (p2.pos.x == newCoord.x && p2.pos.y == newCoord.y && p2.team == p.team)
                                {
                                    possible = false;
                                }
                            }
                            if (possible)
                            {
                                cF.Add(newCoord);
                            }
                            break;
                    }
                }

            }
            return cF;
        }
        public bool IsInCheck(bool team, List<Piece> pieces)
        {
            bool isInCheck = false;

            Piece king = new Piece();
            foreach (Piece p in pieces)
            {
                if (p.team == team && p.type == 'k')
                {
                    king = p;
                }
            }
            foreach (Piece p in pieces)
            {
                if (p.team != team)
                {
                    foreach (Coordinate c in MovementPossibilities(p, pieces))
                    {
                        if (c.x == king.pos.x && c.y == king.pos.y)
                        {
                            if (p.type == 'p')
                            {
                                if (c.x != 0)
                                    return true;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return isInCheck;
        }
        public List<Coordinate> FinalPossibilities(List<Piece> pieces, Piece p)
        {

            Piece pAux = p.DeepClone();
            Piece enemyRemoved = new Piece();
            List<Coordinate> ps = MovementPossibilities(p, pieces);
            List<Coordinate> psF = new List<Coordinate>();

            Piece takenPiece = new Piece();

            foreach (Coordinate c in ps)
            {
                bool removed = false;
                foreach (Piece pp in pieces)
                {
                    if (pp.pos.x == c.x && pp.pos.y == c.y)
                    {
                        enemyRemoved = pieces.Find(p2 => p2.pos.x == c.x && p2.pos.y == c.y).DeepClone();
                        takenPiece = pieces.Find(p2 => p2.pos.x == c.x && p2.pos.y == c.y);
                        takenPiece.pos.x = 50;
                        takenPiece.pos.y = -2;
                        //pieces.RemoveAll(p2 => p2.pos.x == c.x && p2.pos.y == c.y);
                        removed = true;
                        break;
                    }
                }


                p.pos.x = c.x;
                p.pos.y = c.y;

                if (IsInCheck(p.team, pieces))
                {

                }
                else
                {
                    psF.Add(c);
                }
                p.pos.x = pAux.pos.x;
                p.pos.y = pAux.pos.y;

                if (removed)
                {
                    takenPiece.pos.x = enemyRemoved.pos.x;
                    takenPiece.pos.y = enemyRemoved.pos.y;
                }

            }

            return psF;
        }
        public List<Coordinate> IncludeCastling(List<Piece> pieces, Piece king)
        {
            List<Piece> piecesAux = new List<Piece>();
            Piece kingAux = new Piece();

            List<Coordinate> lF = FinalPossibilities(pieces, king);

            if (!IsInCheck(king.team, pieces))
            {
                if (!king.hasMoved)
                {

                    foreach (Piece p in pieces)
                    {
                        piecesAux.Add(p.DeepClone());
                    }
                    foreach (Piece p in piecesAux)
                    {
                        if (p.type == 'k' && p.team == king.team)
                        {
                            kingAux = p;
                        }
                    }
                    foreach (Piece p in pieces)
                    {
                        if (p.type == 'r' && !p.hasMoved && p.team && king.team)
                        {
                            if (p.pos.x == 8 && p.pos.y == 8)
                            {
                                kingAux.pos.x = 6;
                                kingAux.pos.y = 8;
                                if (PieceFromCoordinate(new Coordinate(6, 8)) == null && !IsInCheck(kingAux.team, piecesAux))
                                {
                                    kingAux.pos.x = 7;
                                    kingAux.pos.y = 8;
                                    if (PieceFromCoordinate(new Coordinate(7, 8)) == null && !IsInCheck(kingAux.team, piecesAux))
                                    {
                                        lF.Add(new Coordinate(7, 8));
                                    }
                                }

                            }
                            if (p.pos.x == 1 && p.pos.y == 8)
                            {
                                kingAux.pos.x = 4;
                                kingAux.pos.y = 8;
                                if (PieceFromCoordinate(new Coordinate(4, 8)) == null && !IsInCheck(kingAux.team, piecesAux))
                                {
                                    kingAux.pos.x = 3;
                                    kingAux.pos.y = 8;
                                    if (PieceFromCoordinate(new Coordinate(3, 8)) == null && !IsInCheck(kingAux.team, piecesAux))
                                    {
                                        kingAux.pos.x = 2;
                                        kingAux.pos.y = 8;
                                        if (PieceFromCoordinate(new Coordinate(2, 8)) == null && !IsInCheck(kingAux.team, piecesAux))
                                        {
                                            lF.Add(new Coordinate(3, 8));
                                        }
                                    }
                                }
                            }
                        }
                        if (p.type == 'r' && !p.hasMoved && !p.team && !king.team)
                        {
                            if (p.pos.x == 1 && p.pos.y == 1)
                            {
                                kingAux.pos.x = 6;
                                kingAux.pos.y = 1;
                                if (PieceFromCoordinate(new Coordinate(6, 1)) == null && !IsInCheck(kingAux.team, piecesAux))
                                {
                                    kingAux.pos.x = 7;
                                    kingAux.pos.y = 1;
                                    if (PieceFromCoordinate(new Coordinate(7, 1)) == null && !IsInCheck(kingAux.team, piecesAux))
                                    {
                                        lF.Add(new Coordinate(7, 1));
                                    }
                                }

                            }
                            if (p.pos.x == 8 && p.pos.y == 1)
                            {
                                kingAux.pos.x = 4;
                                kingAux.pos.y = 1;
                                if (PieceFromCoordinate(new Coordinate(4, 1)) == null && !IsInCheck(kingAux.team, piecesAux))
                                {
                                    kingAux.pos.x = 3;
                                    kingAux.pos.y = 1;
                                    if (PieceFromCoordinate(new Coordinate(3, 1)) == null && !IsInCheck(kingAux.team, piecesAux))
                                    {
                                        kingAux.pos.x = 2;
                                        kingAux.pos.y = 1;
                                        if (PieceFromCoordinate(new Coordinate(2, 1)) == null && !IsInCheck(kingAux.team, piecesAux))
                                        {
                                            lF.Add(new Coordinate(3, 1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return lF;
        }
        public static Coordinate SumCoordinate(Coordinate c1, Coordinate c2)
        {
            return new Coordinate(c1.x + c2.x, c1.y + c2.y);
        }
        public static Coordinate MultiplyCoordinate(Coordinate c1, int i)
        {
            return new Coordinate(c1.x * i, c1.y * i);
        }
        public static Coordinate InvertCoordinate(Coordinate c)
        {
            Coordinate cF = new();

            switch (c.x)
            {
                case 8:
                    cF.x = 1;
                    break;
                case 7:
                    cF.x = 2;
                    break;
                case 6:
                    cF.x = 3;
                    break;
                case 5:
                    cF.x = 4;
                    break;
                case 4:
                    cF.x = 5;
                    break;
                case 3:
                    cF.x = 6;
                    break;
                case 2:
                    cF.x = 7;
                    break;
                case 1:
                    cF.x = 8;
                    break;
            }

            switch (c.y)
            {
                case 8:
                    cF.y = 1;
                    break;
                case 7:
                    cF.y = 2;
                    break;
                case 6:
                    cF.y = 3;
                    break;
                case 5:
                    cF.y = 4;
                    break;
                case 4:
                    cF.y = 5;
                    break;
                case 3:
                    cF.y = 6;
                    break;
                case 2:
                    cF.y = 7;
                    break;
                case 1:
                    cF.y = 8;
                    break;
            }

            return cF;
        }
        public static bool IsCoordinateEqual(Coordinate c1, Coordinate c2)
        {
            if (c1.x == c2.x && c1.y == c2.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Coordinate PieceCoord(Piece piece)
        {
            return new Coordinate(piece.pos.x, piece.pos.y);
        }
        public bool TeamOfPieceInCoord(Coordinate c1, List<Piece> pieces)
        {
            foreach (Piece piece in pieces)
            {
                if (c1.x == piece.pos.x && c1.y == piece.pos.y)
                {
                    return piece.team;
                }
            }
            return false;
        }
        public Piece PieceFromCoordinate(Coordinate c)
        {
            foreach (Piece piece in pieces)
            {
                if (c.x == piece.pos.x && c.y == piece.pos.y)
                {
                    return piece;
                }
            }
            
            return null;
        }


    }
}
