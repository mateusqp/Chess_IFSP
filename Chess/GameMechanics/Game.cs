using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    class Game
    {
        Notation notation;
        Timer timer;
        Board board;
        Player player1;
        Player player2;
        bool player1Color;

        public Game(Timer timer, bool player1Color, Player player1, Player player2)
        {
            this.timer = timer;
            board = new Board(player1Color);
            this.player1 = player1;
            this.player2 = player2;
        }
    }
}
