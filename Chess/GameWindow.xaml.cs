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
using SuperSimpleTcp;
using System.Windows.Threading;
using System.Threading;

namespace Chess
{
    /// <summary>
    /// Lógica interna para Game.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        //- Receive these from MainWindow
        Chess.GameMechanics.ChessTimer timer { get; set; }
        SimpleTcpClient client { get; set; }
        bool promotion { get; set; }
        //-
        public GameMechanics.Game game { get; set; }
        bool viewer { get; set; }
        bool v_turn { get; set; } //viewer turn

        Player p1 { get; set; }
        Player p2 { get; set; }

        WaitingRoom wr { get; set; }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        string login;

        List<Piece> promotionPieces = new List<Piece>();

        public GameWindow(SimpleTcpClient client, User user1, User user2, bool player1Color, WaitingRoom wr, bool viewer)
        {
            InitializeComponent();

            this.login = wr.user1.login;

            wr.btnFindMatch.IsEnabled = false;
            wr.btnStopFindMatch.Visibility = Visibility.Collapsed;

            grid.Children.Remove(drawBtn);
            grid.Children.Remove(resignBtn);

            this.wr = wr;
            this.viewer = viewer;
            timer = new Chess.GameMechanics.ChessTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            timerBlack.Content = timer.TimeString(timer.p2Time);
            timerWhite.Content = timer.TimeString(timer.p1Time);

            p1 = new Player();
            p2 = new Player();

            if (!viewer)
            {
                SoundPlayer audio = new SoundPlayer(Properties.Resources.c_START);
                audio.Play();
            }

            promotion = false;

            this.client = client;

            client.Events.Connected += Connected;
            client.Events.Disconnected += Disconnected;
            client.Events.DataReceived += DataReceived;

            DrawBoard();

            if (!player1Color)
            {
                timerBlack.SetValue(Grid.RowProperty, 7);
                timerWhite.SetValue(Grid.RowProperty, 0);
            }

            p1.username = user1.login;
            p1.rating = user1.rating;
            p2.username = user2.login; /////// USE NAME INSTEAD AFTER SQL ADJUSTING
            p2.rating = user2.rating;

            game = new GameMechanics.Game(timer, player1Color, p1, p2);
            

            //MessageBox.Show(user1.login + " " + user1.password);

            game.running = true;
            DrawPiecesOnBoard(game.board.pieces);
            game.InvisibleCells();
            DrawCells(game);

        }
        void Connected(object sender, ConnectionEventArgs e)
        {
            
        }

        void Disconnected(object sender, ConnectionEventArgs e)
        {
            
        }
        void DataReceived(object sender, DataReceivedEventArgs e)
        {
            
            string data = (Encoding.UTF8.GetString(e.Data));

            Dispatcher.Invoke(new Action(delegate
            {
                GetData(data, e.IpPort);
                
            }));
        }

        void GetData(string data, string userIP) ////////////////////////////////// GET DATA /////////////////////////////////////
        {
            
            if (data.Contains("gameResignR*") && game.running)
            {                
                game.running = false;
                Wait(0.5);
                SoundPlayer audio = new SoundPlayer(Properties.Resources.c_WinByResign);
                audio.Play();                
                MessageBox.Show("Win by resignation");
            }

            //"movePieceR*" + game.board.pieces.Count() + "#" + "!" + timer.p1Time.ToString() + "_" + timer.p2Time.ToString()+1_True;
            if (data.Contains("movePieceR*")) //server reply
            {
                game.hasStarted = true;
                if (game.turn != game.player1Color || viewer)
                {
                    game.board.pieces.Clear();
                    game.board.cells.Clear();
                    data = data.Split('*')[1];
                    timer.p1Time = Convert.ToInt32(data.Split('!')[1].Split('-')[0].Split('_')[0]);
                    //0#!589_599-False
                    timer.p2Time = Convert.ToInt32(data.Split('!')[1].Split('-')[0].Split('_')[1]);
                    string turnIntS = data.Split('-')[1];
                    game.turnInt = Convert.ToInt32(turnIntS);
                    data = data.Split('!')[0];
                    int noPecas = Convert.ToInt32(data.Split('#')[0]);
                    data = data.Split('#')[1];

                    if (game.turnInt % 2 == 0)
                    {
                        v_turn = false;
                    }
                    else
                    {
                        v_turn = true;
                    }

                    data = data.Split('!')[0];
                    

                    sbyte x = -1;
                    sbyte y = -1;
                    char type = '-';
                    bool hasMoved = true;
                    bool possibleEnPassant = true;
                    bool team = true;

                    int nextPiece = 6;
                    for (int i = 0; i < noPecas * nextPiece; i++)
                    {
                        
                        int j = i % 6;
                        switch (j)
                        {
                            case 0:
                                x = (sbyte)Char.GetNumericValue(data[i]);
                                break;
                            case 1:
                                y = (sbyte)Char.GetNumericValue(data[i]);
                                break;
                            case 2:
                                type = data[i];
                                break;
                            case 3:
                                if ((int)Char.GetNumericValue(data[i]) == 1)
                                {
                                    hasMoved = true;
                                }
                                else
                                {
                                    hasMoved = false;
                                }
                                break;

                            case 4:
                                if ((int)Char.GetNumericValue(data[i]) == 1)
                                {
                                    possibleEnPassant = true;
                                }
                                else
                                {
                                    possibleEnPassant = false;
                                }
                                break;
                            case 5:
                                if ((int)Char.GetNumericValue(data[i]) == 1)
                                {
                                    team = true;
                                }
                                else
                                {
                                    team = false;
                                }
                                Coordinate newC = new Coordinate(x, y);
                                
                                //MessageBox.Show(team.ToString());
                                Piece newP = new Piece(type, team, newC, possibleEnPassant, hasMoved);                                
                                game.board.pieces.Add(newP);
                                break;
                        }

                    }
                    //wholeBoard += p.pos.x + p.pos.y + p.type + hasMoved + possibleEnPassant + team;
                    if (game.turn && !promotion)
                    {
                        game.turn = false;
                    }
                    else
                    {
                        game.turn = true;
                    }
                    game.InvisibleCells();
                    MovePieceSound();
                    ReDraw();
                    foreach (Piece pp in game.board.pieces)
                    {
                        if (pp.team == game.turn)
                        {
                            CheckForMateOrStale(game.turn, game.board.pieces, pp);
                            break;
                        }
                    }
                    
                    //MessageBox.Show("pieces" + game.board.pieces.Count());
                }
            }

            //MessageBox.Show("User name:" + user.name);
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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
                    if (!game.player1Color && piece.pos.x != 10)
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

            if (c.x != 10) //Promotion pieces
            {
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
            }
            else
            {
                cF = c;
            }
            

            return cF;
        }
        public void DrawCells(GameMechanics.Game game) // -- NOT THE BOARD CELLS, Cells for CLICKABLE rectangles, Also adds the Rectangles///
        {
            game.board.clickableRects.Clear();
            if (!viewer)
            {
                foreach (Cell cell in game.board.cells)
                {
                    Rectangle r = new Rectangle();
                    SolidColorBrush brush = new SolidColorBrush();
                    Coordinate invertCoord = new Coordinate(cell.x + 1, cell.y + 1);
                    if (!game.player1Color && cell.x < 9)
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
                        r.Opacity = 0.2;

                    }
                    if (cell.color == "Blue")
                    {
                        brush.Color = Colors.LightBlue;
                        r.Fill = brush;
                        r.Opacity = 0.4;

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
            
        }

        private void ClickOnPiece(object sender, MouseButtonEventArgs e)
        {
            if (game.player1Color == game.turn && !viewer)
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
                        if (clickedPiece.type == 'k')
                        {
                            game.RedCells(game.board.IncludeCastling(game.board.pieces, clickedPiece));
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
                        

                        promotion = MovePiece(p, clickedCoordinate, game.board);
                        

                        if (!promotion)
                        {
                            if (game.turn)
                            {
                                game.turn = false;
                            }
                            else
                            {
                                game.turn = true;
                            }

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
                                if (p.type == 'p' && p.pos.y == 8)
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

                        SendThisPosition();

                        CheckForMateOrStale(team2, game.board.pieces, clickedPiece);

                        if (game.turn)
                        {
                            game.turn = false;
                        }
                        else
                        {
                            game.turn = true;
                        }

                    }
                }
                
            }
        }
        bool MovePiece(Piece p, Coordinate c, Board board) //returns false if promotion
        {
            if (game.player1Color != game.turn)
            {
                
            }

            game.hasStarted = true;

            if (!promotion)
            {
                bool castling = false;
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
                if (p.type == 'k' && Math.Abs(p.pos.x - c.x) > 1)
                {
                    castling = true;
                    if (p.team)
                    {
                        if (c.x == 7)
                        {
                            Piece rook = game.board.PieceFromCoordinate(new Coordinate(8, 8));
                            rook.pos.x = 6;
                        }
                        if (c.x == 3)
                        {
                            Piece rook = game.board.PieceFromCoordinate(new Coordinate(1, 8));
                            rook.pos.x = 4;
                        }
                    }
                    else
                    {
                        if (c.x == 7)
                        {
                            Piece rook = game.board.PieceFromCoordinate(new Coordinate(8, 1));
                            rook.pos.x = 6;
                        }
                        if (c.x == 3)
                        {
                            Piece rook = game.board.PieceFromCoordinate(new Coordinate(1, 1));
                            rook.pos.x = 4;
                        }
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
                if (!castling)
                {
                    if (hasToRemove)
                    {
                        board.pieces.RemoveAll(p2 => (int)p2.pos.x == xToRemove && (int)p2.pos.y == yToRemove);
                        TakePieceSound();
                    }
                    else
                    {
                        MovePieceSound();
                    }
                }
                else
                {
                    MovePieceSound();
                    TakePieceSound();
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
                                    promotion = true;
                                    game.state = "promotion";
                                    Promotion(p.team, p);                                    
                                    return true;
                                }
                            }
                            else
                            {
                                if (p.pos.y == 8)
                                {
                                    promotion = true;
                                    game.state = "promotion";
                                    Promotion(p.team, p);
                                    return true;
                                }
                            }
                        }
                    }

                }
                CheckForMateOrStale(team2, game.board.pieces, p);
            }

            if (game.state != "promotion")
            {
                SendThisPosition();
            }

            return false;
        }
        public void CheckForMateOrStale(bool team2, List<Piece> pieces, Piece p)
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
            game.running = false;
            StalemateSound();
            if (game.player1Color && !viewer)
            {
                SendThisPosition();
                Wait(1);
                client.Send("gameFinished*none");
                
            }
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
                if (!viewer)
                {
                    CheckmateSound();
                    dispatcherTimer.Stop();
                    if (game.player1Color)
                    {
                        SendThisPosition();
                        Wait(1);
                        client.Send("gameFinished*white");
                        game.running = false;
                    }
                }                                
                MessageBox.Show("Checkmate, white wins!");
            }
            else
            {
                if (!viewer)
                {
                    CheckmateSound();
                    dispatcherTimer.Stop();
                    if (!game.player1Color)
                    {
                        SendThisPosition();
                        Wait(1);
                        client.Send("gameFinished*black");
                        game.running = false;
                    }
                    MessageBox.Show("Checkmate, black wins!");
                }
                
            }
        }
        private void StalemateSound()
        {
            dispatcherTimer.Stop();
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
            if (game.running && !viewer)
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

        async void SendThisPosition()
        {
            string wholeBoard = "";

            foreach (Piece pieceMoving in game.board.pieces)
            {

                int hasMoved, possibleEnPassant, team;
                if (pieceMoving.hasMoved)
                {
                    hasMoved = 1;
                }
                else
                {
                    hasMoved = 0;
                }
                if (pieceMoving.possiblePassant)
                {
                    possibleEnPassant = 1;
                }
                else
                {
                    possibleEnPassant = 0;
                }
                if (pieceMoving.team)
                {
                    team = 1;
                }
                else
                {
                    team = 0;
                }
                wholeBoard += pieceMoving.pos.x.ToString() + pieceMoving.pos.y.ToString() + pieceMoving.type + hasMoved.ToString() + possibleEnPassant.ToString() + team.ToString();
            }
            if (!viewer)
            {
                game.turnInt++;
            }
            await client.SendAsync("movePiece*" + game.board.pieces.Count() + "#" + wholeBoard + "!" + timer.p1Time.ToString() + "_" + timer.p2Time.ToString() + "-" + game.turnInt);
            ///////////////////////////////////////666666666666
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (game.hasStarted && game.running && !viewer)
            {
                if (game.turn)
                {
                    timer.p1Time--;
                }
                else
                {
                    timer.p2Time--;
                }
            }

            if (game.hasStarted && viewer && game.running)
            {
                if (v_turn)
                {
                    timer.p1Time--;
                }
                else
                {
                    timer.p2Time--;
                }
            }

            timerBlack.Content = timer.TimeString(timer.p2Time);
            timerWhite.Content = timer.TimeString(timer.p1Time);

            if (timer.p1Time == 0 && !viewer)
            {
                dispatcherTimer.Stop();
                if (game.player1Color)
                {
                    SendThisPosition();
                    Wait(1);
                    client.Send("gameFinished*white");
                    game.running = false;
                }                
            }
            if (timer.p2Time == 0 && !viewer)
            {
                dispatcherTimer.Stop();
                if (!game.player1Color)
                {
                    SendThisPosition();
                    Wait(1);
                    client.Send("gameFinished*black");
                    game.running = false;
                }
                
            }
        }
        /// <summary>
        /// WPF Wait
        /// </summary>
        /// <param name="seconds"></param>
        public static void Wait(double seconds) //https://stackoverflow.com/questions/21547678/making-a-thread-wait-two-seconds-before-continuing-in-c-sharp-wpf
        {
            var frame = new DispatcherFrame();
            new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
                frame.Continue = false;
            })).Start();
            Dispatcher.PushFrame(frame);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (game.player1Color && game.running && !viewer)
            {
                SendThisPosition();
                Wait(0.5);
                client.Send("gameFinishedResign*black");
                Wait(0.3);
                client.Send("UpdateRating*" + login);
            }
            if (!game.player1Color && game.running && !viewer)
            {
                SendThisPosition();
                Wait(0.5);
                client.Send("gameFinishedResign*white");
                Wait(0.3);
                client.Send("UpdateRating*" + login);
            }
            wr.btnFindMatch.IsEnabled = true;
            wr.Show();
            wr.gameHasStarted = false;
        }
    }
}
