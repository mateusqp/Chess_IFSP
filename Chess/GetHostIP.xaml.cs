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
    /// Lógica interna para GetHostIP.xaml
    /// </summary>
    public partial class GetHostIP : Window
    {
        MainWindow main;
        public GetHostIP(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            main.hostIP_ = txtIP.Text;
            main.Show();
            this.Close();            
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            if(txtIP.Text == "")
            main.Close();
        }
    }
}
