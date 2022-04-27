using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Board
    {
        public List<Piece> pieces;

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
                                pieces.Add(new Piece(type, false, new Square((sbyte)x, (sbyte)y)));
                                break;

                            case 1:
                                pieces.Add(new Piece(type, true, new Square((sbyte)x, (sbyte)y)));
                                break;

                            case 7:
                                pieces.Add(new Piece('p', false, new Square((sbyte)x, (sbyte)y)));
                                break;

                            case 2:
                                pieces.Add(new Piece('p', true, new Square((sbyte)x, (sbyte)y)));
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
                                pieces.Add(new Piece(type, true, new Square((sbyte)x, (sbyte)y)));
                                break;

                            case 1:
                                pieces.Add(new Piece(type, false, new Square((sbyte)x, (sbyte)y)));
                                break;

                            case 7:
                                pieces.Add(new Piece('p', true, new Square((sbyte)x, (sbyte)y)));
                                break;

                            case 2:
                                pieces.Add(new Piece('p', false, new Square((sbyte)x, (sbyte)y)));
                                break;
                        }

                    }
                }
            }
        }
    }
}
