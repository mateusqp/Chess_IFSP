using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Game
    {
        public Notation notation { get; set; }
        public ChessTimer timer { get; set; }
        public Board board { get; set; }
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public bool player1Color { get; set; }
        public bool running { get; set; }
        public bool turn { get; set; } //false = black's turn; true = white's turn
        public string state { get; set; } //selectPiece, selectCoordinate
        public bool hasStarted { get; set; } //first move
        public int turnInt { get; set; }

        public Game(ChessTimer timer, bool player1Color, Player player1, Player player2)
        {
            this.timer = timer;
            board = new Board(player1Color);
            this.player1Color = player1Color;
            this.player1 = player1;
            this.player2 = player2;
            turn = true;
            state = "selectPiece";
            hasStarted = false;
            turnInt = 1;
        }

        #region Cells
        // -- Cells (For clicking events in the board) - -//

        // -- Cells: Opacity, Color, indicating piece movements, or clickable stuff in the grid --//        
        public void InvisibleCells()
        {
            board.cells.Clear();
            foreach (Piece p in board.pieces)
            {
                if (turn == p.team)
                {
                    board.cells.Add(new Cell(p.pos.x, p.pos.y, "Invisible"));
                }
            }
        }
        // --  Creates red cells - possibilities of movements and removes other clickable cells; --//
        public void RedCells(List<Coordinate> coordinates)
        {
            board.cells.Clear();
            foreach (Coordinate redCoordinate in coordinates)
            {
                board.cells.Add(new Cell(redCoordinate.x, redCoordinate.y, "Red"));
            }
        }
        // -- Self-Coordinate method creates a blue cell for cancelling movement --//
        public void BlueCells(Piece piece)
        {
            board.cells.Add(new Cell(piece.pos.x, piece.pos.y, "Blue"));
        }
        // ------------------------------------------------------------//
        // -- Self-Coordinate method creates a blue cell for cancelling movement --//
        public void BluePromotionCells(List<Piece> ps)
        {
            foreach (Piece piece in ps)
            {
                board.cells.Add(new Cell(piece.pos.x, piece.pos.y, "BluePromotion"));
            }
        }
        // ------------------------------------------------------------//
        #endregion
    }
}
