using System;
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
using Force.DeepCloner;

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
        bool promotion = false;
        //-
        public Game game = new Game(timer, true, p1, p2);


        List<Piece> promotionPieces = new List<Piece>();

        public GameWindow()
        {
            InitializeComponent();
            DrawBoard();

            game.running = true;
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
                    retang.Fill = new SolidColorBrush(Color.FromRgb(190, 219, 145));
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
                    retang.Fill = new SolidColorBrush(Color.FromRgb(246, 252, 237));
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

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }

                    Grid.SetRow(wr, invertCoord.y - 1);
                    Grid.SetColumn(wr, invertCoord.x - 1);



                }

                if (piece.type == 'r' && piece.team == false && piece.exists == true)
                {
                    Image br = new Image();
                    br.Source = bri;
                    grid.Children.Add(br);
                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(br, invertCoord.y - 1);
                    Grid.SetColumn(br, invertCoord.x - 1);
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

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(wn, invertCoord.y - 1);
                    Grid.SetColumn(wn, invertCoord.x - 1);

                }

                if (piece.type == 'n' && piece.team == false && piece.exists == true)
                {
                    Image bn = new Image();
                    bn.Source = bni;
                    grid.Children.Add(bn);

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(bn, invertCoord.y - 1);
                    Grid.SetColumn(bn, invertCoord.x - 1);
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

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(wb, invertCoord.y - 1);
                    Grid.SetColumn(wb, invertCoord.x - 1);

                }

                if (piece.type == 'b' && piece.team == false && piece.exists == true)
                {
                    Image bb = new Image();
                    bb.Source = bbi;
                    grid.Children.Add(bb);

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(bb, invertCoord.y - 1);
                    Grid.SetColumn(bb, invertCoord.x - 1);
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

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(wq, invertCoord.y - 1);
                    Grid.SetColumn(wq, invertCoord.x - 1);

                }

                if (piece.type == 'q' && piece.team == false && piece.exists == true)
                {
                    Image bq = new Image();
                    bq.Source = bqi;
                    grid.Children.Add(bq);

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(bq, invertCoord.y - 1);
                    Grid.SetColumn(bq, invertCoord.x - 1);
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

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(wk, invertCoord.y - 1);
                    Grid.SetColumn(wk, invertCoord.x - 1);

                }

                if (piece.type == 'k' && piece.team == false && piece.exists == true)
                {
                    Image bk = new Image();
                    bk.Source = bki;
                    grid.Children.Add(bk);

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(bk, invertCoord.y - 1);
                    Grid.SetColumn(bk, invertCoord.x - 1);
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

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(wp, invertCoord.y - 1);
                    Grid.SetColumn(wp, invertCoord.x - 1);

                }

                if (piece.type == 'p' && piece.team == false && piece.exists == true)
                {
                    Image bp = new Image();
                    bp.Source = bpi;
                    grid.Children.Add(bp);

                    Coordinate invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    if (!game.player1Color)
                    {
                        invertCoord = InvertCoordinate(invertCoord);
                    }
                    else
                    {
                        invertCoord = new Coordinate(piece.pos.x, piece.pos.y);
                    }
                    Grid.SetRow(bp, invertCoord.y - 1);
                    Grid.SetColumn(bp, invertCoord.x - 1);
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
            Coordinate cF = new Coordinate();

            switch (c.x)
            {
                case 9:
                    cF.x = 0;
                    break;
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
                case 0:
                    cF.x = 9;
                    break;
            }

            switch (c.y)
            {
                case 9:
                    cF.y = 0;
                    break;
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
                case 0:
                    cF.y = 9;
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
                Coordinate invertCoord = new Coordinate(cell.x + 1, cell.y + 1);
                if (!game.player1Color)
                {
                    invertCoord = InvertCoordinate(invertCoord);
                }
                else
                {
                    invertCoord = new Coordinate(cell.x - 1, cell.y - 1);
                }
                Grid.SetColumn(r, invertCoord.x);
                Grid.SetRow(r, invertCoord.y);


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
                    r.Opacity = 0.1;

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
            foreach (Piece p in game.board.pieces)
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

            if (game.running && !promotion)
            {
                if (game.state == "selectPiece" && cellColor == "Invisible")
                {
                    foreach (Piece p in game.board.pieces)
                    {
                        if (p.pos.x == clickedCoordinate.x && p.pos.y == clickedCoordinate.y)
                        {
                            p.isClicked = true;
                        }
                    }
                    game.state = "selectCoordinate";
                    game.board.cells.Clear();
                    game.board.clickableRects.Clear();
                    game.RedCells(game.board.FinalPossibilities(game.board.pieces, clickedPiece));

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
                    if (game.turn)
                    {
                        game.turn = false;
                    }
                    else
                    {
                        game.turn = true;
                    }
                    promotion = MovePiece(p, clickedCoordinate, game.board);
                    if (!promotion)
                    {
                        game.state = "selectPiece";
                        game.board.cells.Clear();
                        game.board.clickableRects.Clear();
                        game.InvisibleCells();

                        ReDraw();
                    }
                }
            }
            if (game.running && promotion)
            {
                foreach (Piece p in promotionPieces)
                {
                    if (p.pos.x == clickedCoordinate.x && p.pos.y == clickedCoordinate.y)
                    {
                        clickedPiece = p.DeepClone();
                    }
                }
                if (game.state == "promotion" && cellColor == "BluePromotion")
                {
                    foreach (Piece p in game.board.pieces)
                    {
                        if (p.team)
                        {
                            if (p.type == 'p' && p.pos.y == 1)
                            {
                                p.type = clickedPiece.type;
                            }
                        }
                        else
                        {
                            if (p.type == 'p' && p.pos.y == 9)
                            {
                                p.type = clickedPiece.type;
                            }
                        }
                    }

                    promotion = false;
                    promotionPieces.Clear();
                    game.board.cells.Clear();
                    game.board.clickableRects.Clear();
                    bool team2 = true;
                    game.InvisibleCells();
                    ReDraw();
                    if (clickedPiece.team)
                    {
                        team2 = false;
                    }
                    game.state = "selectPiece";
                    CheckForMateOrStale(team2, game.board.pieces, clickedPiece);
                }
            }

        }
        bool MovePiece(Piece p, Coordinate c, Board board) //returns false if promotion
        {
            if (!promotion)
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

                bool team2 = false; ;

                if (p.team)
                {
                    team2 = false;
                }
                else
                {
                    team2 = true;
                }

                foreach (Piece piece in game.board.pieces)
                {
                    if (piece.team == p.team)
                    {
                        if (p.type == 'p')
                        {
                            if (p.team)
                            {
                                if (p.pos.y == 1)
                                {
                                    Promotion(p.team, p);
                                    return true;
                                }
                            }
                            else
                            {
                                if (p.pos.y == 9)
                                {
                                    Promotion(p.team, p);
                                    return true;
                                }
                            }
                        }
                    }

                }

                CheckForMateOrStale(team2, game.board.pieces, p);
            }

            return false;
        }
        void CheckForMateOrStale(bool team2, List<Piece> pieces, Piece p)
        {
            if (game.board.IsInCheck(team2, game.board.pieces)) //CHECKMATE
            {
                List<Coordinate> lF = new List<Coordinate>();

                foreach (Piece p2 in game.board.pieces)
                {

                    if (p2.team == team2)
                    {
                        foreach (Coordinate c2 in game.board.FinalPossibilities(game.board.pieces, p2))
                        {
                            lF.Add(c2);
                        }
;
                    }
                }

                if (lF.Count == 0)
                {
                    Checkmate(p.team);
                }
            }
            else
            {//STALEMATE
                List<Coordinate> lF = new List<Coordinate>();

                foreach (Piece p2 in game.board.pieces)
                {
                    if (p2.team == team2)
                    {
                        foreach (Coordinate c2 in game.board.FinalPossibilities(game.board.pieces, p2))
                        {
                            lF.Add(c2);
                        }
;
                    }
                }

                if (lF.Count == 0)
                {
                    Stalemate();
                }
            }
        }
        private void Promotion(bool team, Piece p)
        {
            //Removes buttons to give space for promotion
            grid.Children.Clear();
            grid.Children.Add(timerBlack);
            grid.Children.Add(timerWhite);
            game.board.cells.Clear();
            game.board.clickableRects.Clear();

            //Creates pieces
            //White
            Piece wq = new Piece('q', true, new Coordinate(10, 2));
            Piece wn = new Piece('n', true, new Coordinate(10, 3));
            Piece wr = new Piece('r', true, new Coordinate(10, 4));
            Piece wb = new Piece('b', true, new Coordinate(10, 5));
            //
            //White
            Piece bq = new Piece('q', false, new Coordinate(10, 2));
            Piece bn = new Piece('n', false, new Coordinate(10, 3));
            Piece br = new Piece('r', false, new Coordinate(10, 4));
            Piece bb = new Piece('b', false, new Coordinate(10, 5));
            if (team)
            {
                promotionPieces.Add(wq);
                promotionPieces.Add(wn);
                promotionPieces.Add(wr);
                promotionPieces.Add(wb);
            }
            else
            {
                promotionPieces.Add(bq);
                promotionPieces.Add(bn);
                promotionPieces.Add(br);
                promotionPieces.Add(bb);
            }

            DrawBoard();
            DrawPiecesOnBoard(game.board.pieces);
            DrawPiecesOnBoard(promotionPieces);
            game.BluePromotionCells(promotionPieces);
            DrawCells(game);
            game.state = "promotion";
        }
        private void Stalemate()
        {
            StalemateSound();
            grid.Children.Remove(drawBtn);
            grid.Children.Remove(resignBtn);
            MessageBox.Show("Stalemate, draw!");
            game.state = "over";
        }
        private void Checkmate(bool winner)
        {
            grid.Children.Remove(drawBtn);
            grid.Children.Remove(resignBtn);
            game.running = false;
            game.state = "over";
            if (winner) //white
            {
                CheckmateSound();
                MessageBox.Show("Checkmate, white wins!");
            }
            else
            {
                CheckmateSound();
                MessageBox.Show("Checkmate, black wins!");
            }
        }
        private void StalemateSound()
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.stalemate);
            audio.Play();
        }
        private void TakePieceSound()
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.piece_fall);
            audio.Play();
        }
        private void CheckmateSound()
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.checkmate);
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
            if (game.running)
            {
                grid.Children.Add(drawBtn);
                grid.Children.Add(resignBtn);
            }
            grid.Children.Add(timerBlack);
            grid.Children.Add(timerWhite);
            DrawBoard();
            DrawPiecesOnBoard(game.board.pieces);
            DrawCells(game);
        }

    }
}
