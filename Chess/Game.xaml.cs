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




namespace Chess
{
    /// <summary>
    /// Lógica interna para Game.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Resources resources;

        public GameWindow()
        {
            InitializeComponent();
            DrawBoard();

            //- Receive these from MainWindow
            Timer timer = new Timer();
            bool player1Color = true;
            Player p1 = new();
            Player p2 = new();
            //-

            Game game = new Game(timer, player1Color, p1, p2);

            DrawPiecesOnBoard(game.board.pieces);
            
        }
        
        private void Offer_Draw(object sender, RoutedEventArgs e)
        {

        }

        private void Resign(object sender, RoutedEventArgs e)
        {

        }
        
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


    }
}
