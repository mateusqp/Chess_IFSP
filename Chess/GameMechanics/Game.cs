using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Game
    {
        public Notation notation;
        public Timer timer;
        public Board board;
        public Player player1;
        public Player player2;
        public bool player1Color;
        public bool running;

        public Game(Timer timer, bool player1Color, Player player1, Player player2)
        {
            this.timer = timer;
            board = new Board(player1Color);
            this.player1 = player1;
            this.player2 = player2;
        }
    }
}
