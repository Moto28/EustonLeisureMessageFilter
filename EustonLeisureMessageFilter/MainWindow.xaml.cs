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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EustonLeisureMessageFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            //closes the window
            this.Close();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            //opens creates and opens new window 
            CreateMessage newWindow = new CreateMessage();
            newWindow.Show();
            //hide mainWindow
            this.Hide();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //allows the custom window to move 
            DragMove();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            //opens creates and opens new window 
            LoadMessage newWindow = new LoadMessage();
            newWindow.Show();
            //hide mainWindow
            this.Hide();
        }
    }
}
