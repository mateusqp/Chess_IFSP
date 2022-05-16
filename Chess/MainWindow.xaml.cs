using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User user1 = new User();
        User user2 = new User();
        bool player1Color = false;
        public string hostIP_ = "127.0.0.1";

        SimpleTcpClient client;

        public MainWindow()
        {
            InitializeComponent();

            
            user1.name = null;

            

            
            
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

            Dispatcher.Invoke(new Action (delegate
            {
                GetData(data, e.IpPort);
                //richTextBox1.Text += data;
            }));
        }

        void GetData(string data, string userIP) ////////////////////////////////// GET DATA /////////////////////////////////////
        {
            
            if (data.Contains("loginReplyROk*"))
            {
                string usersOnline = data.Split('#')[1];
                data = data.Split('#')[0];
                data = data.Split('*')[1];
                user1.name = data.Split('!')[0];
                user1.rating = Convert.ToInt32(data.Split('!')[1]);
                WaitingRoom waitingRoom = new WaitingRoom(user1, client, usersOnline);
                Hide();
                waitingRoom.Show();
                
            }

            if (data.Contains("loginRFail*"))
            {
                client.Disconnect();
                loginBtn.IsEnabled = true;
            }

            if (data.Contains("userQuitR*"))
            {
                client.Disconnect();
                loginBtn.IsEnabled = true;
            }

            

            //MessageBox.Show("User name:" + user.name);
        }

        private async void login_click(object sender, RoutedEventArgs e)
        {
            
            if (txtLogin.Text.Length > 5 && txtLogin.Text.Length < 13 && txtPass.Text.Length > 5 && txtPass.Text.Length < 13)
            {
                loginBtn.IsEnabled = false;
                hostIP_ = txtHost.Text;
                if (hostIP_ == "")
                {
                    hostIP_ = "127.0.0.1";
                }

                client = new SimpleTcpClient(hostIP_ + ":9000");
                // set events
                client.Events.Connected += Connected;
                client.Events.Disconnected += Disconnected;
                client.Events.DataReceived += DataReceived;
                try
                {
                    client.Connect();
                    await client.SendAsync("login*" + txtLogin.Text + "!" + txtPass.Text);
                    await Task.Delay(1000);
                    if (user1.name == null)
                    {
                        client.Disconnect();
                        loginBtn.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    await Task.Delay(1000);
                    if (user1.name == null)
                    {
                        client.Disconnect();
                        loginBtn.IsEnabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Login/Password must be between 6 to 12 characters");
            }
        }

        private void play_click(object sender, RoutedEventArgs e)
        {

            Hide();

            //GameWindow game = new GameWindow(this, user1);
            //game.Show();
        }

        static string GetLocalIPv4(NetworkInterfaceType _type)
        //GetLocalIPv4(NetworkInterfaceType.Wireless80211);
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        private void txtLogin_changed(object sender, TextChangedEventArgs e)
        {
            if (txtLogin.Text.All(chr => char.IsLetter(chr) || char.IsNumber(chr)))
            {
                
            }
            else
            {
                MessageBox.Show("Special characters are prohibited, only letters and numbers allowed");
                txtLogin.Clear();
            }
        }



        private void txtPass_changed(object sender, TextChangedEventArgs e)
        {
            if (txtPass.Text.All(chr => char.IsLetter(chr) || char.IsNumber(chr)))
            {

            }
            else
            {
                MessageBox.Show("Special characters are prohibited, only letters and numbers allowed");
                txtPass.Clear();
            }
            
        }



        

    }


}
