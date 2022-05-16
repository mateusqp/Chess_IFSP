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
        
        public WaitingRoom(User user, SimpleTcpClient client, string usersOnline)
        {
            InitializeComponent();

            this.client = client;
            user1 = user;

            // set events
            client.Events.Connected += Connected;
            client.Events.Disconnected += Disconnected;
            client.Events.DataReceived += DataReceived;

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
            if (data.Contains("gameStart*"))
            {
                User user2 = new User();

                data = data.Split('*')[1];
                string login2 = data.Split('!')[0];
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

                user2.login = login2;
                user2.rating = Convert.ToInt32(rating2);
                GameWindow game = new GameWindow(client, user1, user2, player1Color);
                game.Show();
            }

            if (data.Contains("gameStart2*")) //JUST FOR TESTING // REMOVE LATER
            {
                User user2 = new User();

                data = data.Split('*')[1];
                string login2 = data.Split('!')[0];
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

                user2.login = login2;
                user2.rating = Convert.ToInt32(rating2);
                GameWindow game = new GameWindow(client, user1, user2, true); //////////////////////TESTING CHANGE COLOR;
                game.Show();
            }

            if (data.Contains("usersOnlineBroadcast*"))
            {
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
            string selectedPlayer = playersList.SelectedItem.ToString();

            if (MessageBox.Show("Do you want to challenge " + selectedPlayer + "?", "Challenge", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                //do yes stuff
            }

        }

        private async void btnFindMatch_Click(object sender, RoutedEventArgs e)
        {
            user1.findingMatch = true;
            btnFindMatch.IsEnabled = false;
            await Task.Delay(1000);
            await client.SendAsync("findingMatch*");
        }
        
    }
}
