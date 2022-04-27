using System;
using System.Collections.Generic;
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


namespace Chess
{
    /// <summary>
    /// Lógica interna para Game.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            DrawBoard();
            Timer timer = new Timer();
            bool player1Color = true;
            Player p1 = new Player();
            Player p2 = new Player();

            Game game = new Game(timer, player1Color, p1, p2);
            
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
        }
    }
}
