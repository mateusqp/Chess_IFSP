using SuperSimpleTcp;
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

namespace Chess
{
    /// <summary>
    /// Lógica interna para WaitingRoom.xaml
    /// </summary>
    public partial class WaitingRoom : Window
    {
        User user1 = new User();
        SimpleTcpClient client;
        public bool gameHasStarted;
        public WaitingRoom(User user, SimpleTcpClient client, string usersOnline)
        {
            InitializeComponent();

            gameHasStarted = false;
            this.client = client;
            user1 = user;

            // set events
            client.Events.Connected += Connected;
            client.Events.Disconnected += Disconnected;
            client.Events.DataReceived += DataReceived;

            System.Windows.Threading.DispatcherTimer loadingLabelTimer = new System.Windows.Threading.DispatcherTimer();
            loadingLabelTimer.Tick += new EventHandler(loadingLabelTimer_Tick);
            loadingLabelTimer.Interval = new TimeSpan(0, 0, 1);
            loadingLabelTimer.Start();

            // GETS USER LIST, RIGHT AFTER LOGIN IN
            string userOnlineFromSend = "";

            for (int i = 0; i < usersOnline.Length; i++)
            {
                if (usersOnline[i] != '-')
                {
                    userOnlineFromSend += usersOnline[i];
                }
                else
                {
                    if (userOnlineFromSend != "")
                    {
                        playersList.Items.Add(userOnlineFromSend);
                    }
                    userOnlineFromSend = "";
                }
            }
            // 

            

        }

        static void Connected(object sender, ConnectionEventArgs e)
        {
            //Console.WriteLine($"*** Server {e.IpPort} connected");
        }

        static void Disconnected(object sender, ConnectionEventArgs e)
        {
            //Console.WriteLine($"*** Server {e.IpPort} disconnected");
            //MessageBox.Show("Disconnected");
        }

        void DataReceived(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine($"[{e.IpPort}] {Encoding.UTF8.GetString(e.Data)}");
            string data = (Encoding.UTF8.GetString(e.Data));

            Dispatcher.Invoke(new Action(delegate
            {
                GetData(data, e.IpPort);
                //richTextBox1.Text += data;
            }));
        }

        void GetData(string data, string userIP) ///////////////////////////////////////GET DATA //////////////////
        {
            if (data.Contains("watchR*"))
            {
                MessageBox.Show("watchR ok");
                data = data.Split('*')[1];
                //sqlName0!sqlName1#rating1_rating2
                string ratingsString = data.Split('#')[1];
                data = data.Split('#')[0];
                labelLoadingGame.Visibility = Visibility.Collapsed;
                User user2 = new User();
                User viewer = new User();

                string vName = data.Split('!')[0];
                //string vRating = data.Split('!')[1];

                viewer.name = vName;
                viewer.rating = Convert.ToInt32(ratingsString.Split('_')[0]);
                viewer.login = "v";
                
                string name2 = data.Split('!')[1];
                
                //string rating2 = data.Split('#')[0];
                

                user2.name = name2;
                user2.rating = Convert.ToInt32(ratingsString.Split('_')[1]);
                GameWindow game = new GameWindow(client, viewer, user2, true, this, true);
                this.Hide();
                game.Show();
                gameHasStarted = true;
            }

            if (data.Contains("challengeR*"))
            {
                string user1Name = data.Split('*')[1];
                if (MessageBox.Show("The user " + user1Name + " is challenging you. Accept Challenge?", "Challenge", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //do no stuff
                    client.Send("challengeReject*" + user1.name); //this is user2 in the challenge tho, careful!
                }
                else
                {
                    //do yes stuff
                    client.Send("challengeAccept*" + user1.name); //this is user2 in the challenge tho, careful!                    
                }
            }

            if (data.Contains("gameStart*"))
            {
                labelLoadingGame.Visibility = Visibility.Collapsed;
                User user2 = new User();

                data = data.Split('*')[1];
                string name2 = data.Split('!')[0];
                data = data.Split('!')[1];
                string rating2 = data.Split('#')[0];
                data = data.Split('#')[1];

                bool player1Color = true;

                if (data == "1")
                {
                    player1Color = true;
                }
                else
                {
                    player1Color = false;
                }

                user2.name = name2;
                user2.rating = Convert.ToInt32(rating2);
                GameWindow game = new GameWindow(client, user1, user2, player1Color, this, false);
                this.Hide();
                game.Show();
                gameHasStarted = true;
            }

            if (data.Contains("gameStart2*")) //JUST FOR TESTING // REMOVE LATER
            {
                labelLoadingGame.Visibility = Visibility.Collapsed;
                User user2 = new User();

                data = data.Split('*')[1];
                string name2 = data.Split('!')[0];
                data = data.Split('!')[1];
                string rating2 = data.Split('#')[0];
                data = data.Split('#')[1];                

                user2.name = name2;
                user2.rating = Convert.ToInt32(rating2);
                GameWindow game = new GameWindow(client, user1, user2, false, this, false); //////////////////////TESTING CHANGE COLOR;
                this.Hide();
                game.Show();
                gameHasStarted = true;
            }

            if (data.Contains("usersOnlineBroadcast*"))
            {
                string _gamesList = data.Split('!')[1];
                List<string> games = new List<string>();

                string a_game = "";
                for (int i = 0; i < _gamesList.Length; i++)
                {
                    
                    if (_gamesList[i] == '#')
                    {
                        games.Add(a_game);
                        a_game = "";
                    }
                    else
                    {
                        a_game += _gamesList[i];
                    }
                }
                gamesList.Items.Clear();

                foreach (string g in games)
                {
                    gamesList.Items.Add(g); 
                }

                data = data.Split('!')[0];
                playersList.Items.Clear();
                //MessageBox.Show("Yep");
                data = data.Split('*')[1];

                string usersOnline = data;

                string userOnlineFromBroadcast = "";

                for (int i = 0; i < usersOnline.Length; i++)
                {
                    if (usersOnline[i] != '-')
                    {
                        userOnlineFromBroadcast += usersOnline[i];
                    }
                    else
                    {
                        if (userOnlineFromBroadcast != "")
                        {
                            playersList.Items.Add(userOnlineFromBroadcast);
                        }
                        userOnlineFromBroadcast = "";
                    }
                }

            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            client.SendAsync("userQuit*");
            client.Disconnect();
            Application.Current.Shutdown();
        }
        private void listBox1_MouseRightClick(object sender, MouseButtonEventArgs e)
        {
            if (!gameHasStarted)
            {
                try
                {
                    if (playersList.SelectedItem != null && playersList.SelectedItem.ToString() != user1.name)
                    {
                        string selectedPlayer = playersList.SelectedItem.ToString();

                        if (MessageBox.Show("Do you want to challenge " + selectedPlayer + "?", "Challenge", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            //do no stuff
                        }
                        else
                        {
                            //do yes stuff
                            client.Send("challenge*" + selectedPlayer);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void btnFindMatch_Click(object sender, RoutedEventArgs e)
        {
            user1.findingMatch = true;
            btnFindMatch.IsEnabled = false;
            await Task.Delay(1000);
            await client.SendAsync("findingMatch*");
            labelLoadingGame.Visibility = Visibility.Visible;
        }

        private void loadingLabelTimer_Tick(object sender, EventArgs e)
        {
            labelLoadingGame.Content += ". ";
            if (labelLoadingGame.Content.ToString() == "Finding Match . . . . ")
            {
                labelLoadingGame.Content = "Finding Match ";
            }
        }

        private void watchGame_click(object sender, MouseButtonEventArgs e)
        {
            if (!gameHasStarted)
            {
                try
                {
                    if (gamesList.SelectedItem != null)
                    {
                        string selectedGame = gamesList.SelectedItem.ToString();

                        if (MessageBox.Show("Do you want to watch " + selectedGame + "?", "Watch game", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            //do no stuff
                        }
                        else
                        {
                            //do yes stuff
                            string gameID = "";
                            for (int i = 0; i < selectedGame.Count(); i++)
                            {
                                if (selectedGame[i] != ' ')
                                {
                                    gameID += selectedGame[i];
                                }
                                else
                                {
                                    i = selectedGame.Count();
                                }
                            }                            
                            client.Send("watch*" + selectedGame);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
