using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Board
    {
        public List<Piece> pieces = new();

        public Board(bool team1) //Standard Board
        {
            if(team1) //Case player 1 is playing white
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
            else //Case player 1 is playing black
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
                                type = 'k';
                                break;
                            case 5:
                                type = 'q';
                                break;
                            case 6:
                                type = 'n';
                                break;
                            case 7:
                                type = 'r';
                                break;
                            case 8:
                                type = 'r';
                                break;

                        }
                        switch (y)
                        {
                            case 8:
                                pieces.Add(new Piece(type, false, new Coordinate((sbyte)x, (sbyte)y)));
                                break;

                            case 1:
                                pieces.Add(new Piece(type, true, new Coordinate((sbyte)x, (sbyte)y)));
                                break;

                            case 7:
                                pieces.Add(new Piece('p', false, new Coordinate((sbyte)x, (sbyte)y)));
                                break;

                            case 2:
                                pieces.Add(new Piece('p', true, new Coordinate((sbyte)x, (sbyte)y)));
                                break;
                        }

                    }
                }
            }
        }

        public static List<Coordinate> PieceMovementVector(Piece p)
        {
            List<Coordinate> c = new();
            switch(p.type)
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
                    if(p.team) //White
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

        public static List<Coordinate> MovementPossibilities(Piece p, List<Piece> pieces)
        {
            List<Coordinate> cF = new(); //Final list

            foreach(Coordinate c in PieceMovementVector(p))
            {
                Coordinate newCoord = new();
                newCoord = SumCoordinate(p.pos, c);
                for(int i = 1; i < 8; i++)
                {
                    if (p.type == 'p' && i > 1)
                        break;
                    if (p.type == 'n' && i > 1)
                        break;
                    if (p.type == 'k' && i > 1)
                        break;

                    newCoord = MultiplyCoordinate(newCoord, i);

                    if (newCoord.x > 1 && newCoord.x < 9 && newCoord.y > 1 && newCoord.y < 9)
                    {
                        foreach(Piece p2 in pieces)
                        {
                            if(p.team == p2.team && newCoord == p2.pos)
                            {
                                i = 9;
                                break;
                            }
                            if (p.team != p2.team && newCoord == p2.pos)
                            {
                                cF.Add(newCoord);
                                i = 9;
                                break;
                            }
                        }
                        if(i < 9)
                        {
                            cF.Add(newCoord);
                        }
                    }

                }

                bool possible;

                if (newCoord.x > 1 && newCoord.x < 9 && newCoord.y > 1 && newCoord.y < 9)
                {
                    switch (p.type)
                    {
                        case 'p':
                            possible = true;
                            foreach (Piece p2 in pieces)
                            {
                                if (c.y == 2 || c.y == -2 && p.hasMoved)
                                {
                                    possible = false;
                                    break;
                                }

                                if (c.y == 2)
                                {
                                    if (p2.pos == newCoord)
                                    {
                                        possible = false;
                                        break;
                                    }
                                    if (p2.pos == new Coordinate(newCoord.x, newCoord.y - 1))
                                    {
                                        possible = false;
                                        break;
                                    }
                                }

                                if (c.y == -2)
                                {
                                    if (p2.pos == newCoord)
                                    {
                                        possible = false;
                                        break;
                                    }
                                    if (p2.pos == new Coordinate(newCoord.x, newCoord.y + 1))
                                    {
                                        possible = false;
                                        break;
                                    }
                                }
                            }

                            if (c.y == 1 || c.y == -1)
                            {
                                foreach (Piece p2 in pieces)
                                {
                                    if (p2.pos == newCoord)
                                    {
                                        possible = false;
                                        break;
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
                                if (p2.pos == newCoord && p2.team == p.team)
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
                                if (p2.pos == newCoord && p2.team == p.team)
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

        public static Coordinate SumCoordinate(Coordinate c1, Coordinate c2)
        {
            return new Coordinate(c1.x + c2.x, c1.y + c2.y);
        }
        public static Coordinate MultiplyCoordinate(Coordinate c1, int i)
        {
            return new Coordinate(c1.x * i, c1.y *i);
        }
    }
}
