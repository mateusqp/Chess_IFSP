﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chess.GameMechanics;
using Chess.Properties;
using System.Media;




namespace Chess
{
    /// <summary>
    /// Lógica interna para Game.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        //- Receive these from MainWindow
        static Timer timer = new Timer();
        static bool player1Color = true;
        static Player p1 = new();
        static Player p2 = new();
        //-
        Game game = new Game(timer, player1Color, p1, p2);


        string gameState = "selectPiece";
        public GameWindow()
        {
            InitializeComponent();
            DrawBoard();


            DrawPiecesOnBoard(game.board.pieces);
            game.InvisibleCells();
            DrawCells(game);            
        }

        
        private void Offer_Draw(object sender, RoutedEventArgs e)
        {

        }

        private void Resign(object sender, RoutedEventArgs e)
        {

        }
        #region BoardDrawing
        public void DrawBoard()
        {

            ////Colored rectangles
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle retang = new Rectangle();
                    retang.Fill = new SolidColorBrush(Color.FromRgb(200, 252, 177));
                    retang.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    retang.StrokeThickness = 2;

                    Grid.SetRow(retang, i);
                    if (i % 2 == 1)
                    {
                        Grid.SetColumn(retang, 2 * j + 0);
                    }
                    else
                    {
                        Grid.SetColumn(retang, 2 * j + 1);
                    }

                    grid.Children.Add(retang);
                }
            }

            ////White rectangle
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle retang = new Rectangle();
                    retang.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    retang.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    retang.StrokeThickness = 2;

                    Grid.SetRow(retang, i);
                    if (i % 2 == 1)
                    {
                        Grid.SetColumn(retang, 2 * j + 1);
                    }
                    else
                    {
                        Grid.SetColumn(retang, 2 * j + 0);
                    }

                    grid.Children.Add(retang);
                }
            }
        } //working
        public void DrawPiecesOnBoard(List<Piece> pieces)
        {

            //White Rook            
            BitmapImage wri = new BitmapImage();
            wri = LoadImage(Properties.Resources.Chess_rlt60);
            //Black Rook (br)
            BitmapImage bri = new BitmapImage();
            bri = LoadImage(Properties.Resources.Chess_rdt60);
            foreach (Piece piece in pieces)
            {
                if (piece.type == 'r' && piece.team == true && piece.exists == true)
                {
                    Image wr = new Image();                    
                    wr.Source = wri;                    
                    grid.Children.Add(wr);
                    Grid.SetRow(wr, piece.pos.y - 1);
                    Grid.SetColumn(wr, piece.pos.x - 1);



                }

                if (piece.type == 'r' && piece.team == false && piece.exists == true)
                {
                    Image br = new Image();
                    br.Source = bri;
                    grid.Children.Add(br);
                    Grid.SetRow(br, piece.pos.y - 1);
                    Grid.SetColumn(br, piece.pos.x - 1);
                }
            }

            //White knight (wn)
            BitmapImage wni = new BitmapImage();
            wni = LoadImage(Properties.Resources.Chess_nlt60);
            //Black Night (bn)
            BitmapImage bni = new BitmapImage();
            bni = LoadImage(Properties.Resources.Chess_ndt60);

            foreach (Piece piece in pieces)
            {
                if (piece.type == 'n' && piece.team == true && piece.exists == true)
                {
                    Image wn = new Image();
                    wn.Source = wni;
                    grid.Children.Add(wn);
                    Grid.SetRow(wn, piece.pos.y - 1);
                    Grid.SetColumn(wn, piece.pos.x - 1);

                }

                if (piece.type == 'n' && piece.team == false && piece.exists == true)
                {
                    Image bn = new Image();
                    bn.Source = bni;
                    grid.Children.Add(bn);
                    Grid.SetRow(bn, piece.pos.y - 1);
                    Grid.SetColumn(bn, piece.pos.x - 1);
                }
            }

            //Bispo Branco (wb)
            BitmapImage wbi = new BitmapImage();
            wbi = LoadImage(Properties.Resources.Chess_blt60);
            //Black Bishop (bb)
            BitmapImage bbi = new BitmapImage();
            bbi = LoadImage(Properties.Resources.Chess_bdt60);

            foreach (Piece piece in pieces)
            {
                if (piece.type == 'b' && piece.team == true && piece.exists == true)
                {
                    Image wb = new Image();
                    wb.Source = wbi;
                    grid.Children.Add(wb);
                    Grid.SetRow(wb, piece.pos.y - 1);
                    Grid.SetColumn(wb, piece.pos.x - 1);

                }

                if (piece.type == 'b' && piece.team == false && piece.exists == true)
                {
                    Image bb = new Image();
                    bb.Source = bbi;
                    grid.Children.Add(bb);
                    Grid.SetRow(bb, piece.pos.y - 1);
                    Grid.SetColumn(bb, piece.pos.x - 1);
                }
            }

            //White Queen (wq)
            BitmapImage wqi = new BitmapImage();
            wqi = LoadImage(Properties.Resources.Chess_qlt60);

            //Black Queen (bq)
            BitmapImage bqi = new BitmapImage();
            bqi = LoadImage(Properties.Resources.Chess_qdt60);

            foreach (Piece piece in pieces)
            {
                if (piece.type == 'q' && piece.team == true && piece.exists == true)
                {
                    Image wq = new Image();
                    wq.Source = wqi;
                    grid.Children.Add(wq);
                    Grid.SetRow(wq, piece.pos.y - 1);
                    Grid.SetColumn(wq, piece.pos.x - 1);

                }

                if (piece.type == 'q' && piece.team == false && piece.exists == true)
                {
                    Image bq = new Image();
                    bq.Source = bqi;
                    grid.Children.Add(bq);
                    Grid.SetRow(bq, piece.pos.y - 1);
                    Grid.SetColumn(bq, piece.pos.x - 1);
                }
            }

            //White King (wk)
            BitmapImage wki = new BitmapImage();
            wki = LoadImage(Properties.Resources.Chess_klt60);

            //Black King (bk)
            BitmapImage bki = new BitmapImage();
            bki = LoadImage(Properties.Resources.Chess_kdt60);

            foreach (Piece piece in pieces)
            {
                if (piece.type == 'k' && piece.team == true && piece.exists == true)
                {
                    Image wk = new Image();
                    wk.Source = wki;
                    grid.Children.Add(wk);
                    Grid.SetRow(wk, piece.pos.y - 1);
                    Grid.SetColumn(wk, piece.pos.x - 1);

                }

                if (piece.type == 'k' && piece.team == false && piece.exists == true)
                {
                    Image bk = new Image();
                    bk.Source = bki;
                    grid.Children.Add(bk);
                    Grid.SetRow(bk, piece.pos.y - 1);
                    Grid.SetColumn(bk, piece.pos.x - 1);
                }
            }

            //White Pawn (wp = White Pawn)
            BitmapImage wpi = new BitmapImage();
            wpi = LoadImage(Properties.Resources.Chess_plt60);
            //Black Pawn (bp)
            BitmapImage bpi = new BitmapImage();
            bpi = LoadImage(Properties.Resources.Chess_pdt60);

            foreach (Piece piece in pieces)
            {
                if (piece.type == 'p' && piece.team == true && piece.exists == true)
                {
                    Image wp = new Image();
                    wp.Source = wpi;
                    grid.Children.Add(wp);
                    Grid.SetRow(wp, piece.pos.y - 1);
                    Grid.SetColumn(wp, piece.pos.x - 1);

                }

                if (piece.type == 'p' && piece.team == false && piece.exists == true)
                {
                    Image bp = new Image();
                    bp.Source = bpi;
                    grid.Children.Add(bp);
                    Grid.SetRow(bp, piece.pos.y - 1);
                    Grid.SetColumn(bp, piece.pos.x - 1);
                }
            }


        }
        #endregion
        private static BitmapImage LoadImage(byte[] imageData) //Thanks to https://stackoverflow.com/users/76051/random-dev
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        public static Coordinate InvertCoordinate(Coordinate c)
        {
            Coordinate cF = new();

            switch(c.x)
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
        public void DrawCells(Game game) // -- NOT THE BOARD CELLS, Cells for CLICKABLE rectangles, Also adds the Rectangles///
        {
            game.board.clickableRects.Clear();

            foreach (Cell cell in game.board.cells)
            {
                Rectangle r = new Rectangle();
                SolidColorBrush brush = new SolidColorBrush();
                Grid.SetColumn(r, cell.x - 1);
                Grid.SetRow(r, cell.y - 1);


                if (cell.color == "Invisible")
                {
                    brush.Color = Colors.White;
                    r.Fill = brush;
                    r.Opacity = 0;

                }
                if (cell.color == "Red")
                {
                    brush.Color = Colors.Red;
                    r.Fill = brush;
                    r.Opacity = 0.15;

                }
                if (cell.color == "Blue")
                {
                    brush.Color = Colors.LightBlue;
                    r.Fill = brush;
                    r.Opacity = 0.2;

                }
                if (cell.color == "BluePromotion")
                {
                    brush.Color = Colors.LightBlue;
                    r.Fill = brush;
                    r.Opacity = 0.2;

                }

                grid.Children.Add(r);

                r.MouseDown += new MouseButtonEventHandler(ClickOnPiece);

                game.board.clickableRects.Add(r);
            }
        }

        private void ClickOnPiece(object sender, MouseButtonEventArgs e)
        {
            //Detects Piece and Coordinate
            Coordinate clickedCoordinate = new Coordinate();
            Piece clickedPiece = new Piece();
            string cellColor = "";

            for (int i = 0; i < game.board.clickableRects.Count; i++)
            {
                if (sender.Equals(game.board.clickableRects[i]))
                {
                    //Gets the clicked piece
                    clickedCoordinate = new Coordinate((int)game.board.cells[i].x, (int)game.board.cells[i].y);                     
                }
            }
            foreach(Piece p in game.board.pieces)
            {
                if (p.pos.x == clickedCoordinate.x && p.pos.y == clickedCoordinate.y)
                {
                    clickedPiece = p;
                }
            }



            foreach (Cell c in game.board.cells)
            {
                if (clickedCoordinate.x == c.x && clickedCoordinate.y == c.y)
                {
                    cellColor = c.color;
                }
            }



            if (game.state == "selectPiece" && cellColor == "Invisible")
            {
                foreach(Piece p in game.board.pieces)
                {
                    if (p.pos.x == clickedCoordinate.x && p.pos.y == clickedCoordinate.y)
                    {
                        p.isClicked = true;
                    }
                }
                game.state = "selectCoordinate";
                game.board.cells.Clear();
                game.board.clickableRects.Clear();
                if (clickedPiece.type == 'k')
                {
                    game.RedCells(game.board.FinalPossibilities(game.board.pieces, clickedPiece));
                    
                }
                else
                {
                    game.RedCells(game.board.FinalPossibilities(game.board.pieces, clickedPiece));
                }
                
                game.BlueCells(clickedPiece);

                ReDraw();
            }

            if (game.state == "selectCoordinate" && cellColor == "Blue")
            {
                foreach (Piece p in game.board.pieces)
                {
                    p.isClicked = false;
                }
                game.state = "selectPiece";
                game.board.cells.Clear();
                game.board.clickableRects.Clear();
                game.InvisibleCells();

                ReDraw();
            }

            if (game.state == "selectCoordinate" && cellColor == "Red")
            {                
                Piece p = new Piece();
                foreach (Piece piece in game.board.pieces)
                {
                    if (piece.isClicked)
                    {
                        p = piece;
                    }
                }
                p.isClicked = false;
                p.hasMoved = true;
                if(game.turn)
                {
                    game.turn = false;
                }
                else
                {
                    game.turn = true;
                }
                MovePiece(p, clickedCoordinate, game.board);
                game.state = "selectPiece";
                game.board.cells.Clear();
                game.board.clickableRects.Clear();
                game.InvisibleCells();

                ReDraw();
            }
            
        }
        void MovePiece(Piece p, Coordinate c, Board board)
        {
            int xToRemove = -1;
            int yToRemove = -1;
            bool hasToRemove = false;
            foreach (Piece piece in board.pieces)
            {
                if (p.team != piece.team)
                {
                    piece.possiblePassant = false;
                }
            }
            if (p.type == 'p')
            {
                if (Math.Abs(p.pos.y - c.y) == 2)
                {
                    p.possiblePassant = true;
                }
            }
            

            foreach (Piece p2 in board.pieces)
            {
                if (p2.pos.x == c.x && p2.pos.y == c.y)
                {
                    xToRemove = c.x;
                    yToRemove = c.y;
                    hasToRemove = true;
                }
            }
            if (hasToRemove)
            {
                board.pieces.RemoveAll(p2 => (int)p2.pos.x == xToRemove && (int)p2.pos.y == yToRemove);
                TakePieceSound();
            }
            else
            {
                MovePieceSound();
            }

            p.pos.x = c.x;
            p.pos.y = c.y;
        }
        private void TakePieceSound()
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.piece_fall); 
            audio.Play();
        }
        private void MovePieceSound()
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.piece_slide);
            audio.Play();
        }
        void ReDraw()
        {
            grid.Children.Clear();
            DrawBoard();
            DrawPiecesOnBoard(game.board.pieces);
            DrawCells(game);
        }
    }
}
