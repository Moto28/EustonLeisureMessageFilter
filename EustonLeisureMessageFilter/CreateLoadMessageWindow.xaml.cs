using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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


namespace EustonLeisureMessageFilter
{

    /// <summary>
    /// Interaction logic for CreateMessage.xaml
    /// </summary>
    public partial class CreateMessage : Window
    {
        Validation valid = new Validation();
        List<Message> CreateMessageList = new List<Message>();
        List<Message> LoadMessageList = new List<Message>();
        int clickCount;

        public CreateMessage()
        {
            InitializeComponent();
            //hides Ui elements at start-up
            subjectLbl.Visibility = Visibility.Hidden;
            subjectTxtBox.Visibility = Visibility.Hidden;
            twitter.Visibility = Visibility.Hidden;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //allows the window to move
            DragMove();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void MessageTypeComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            valid.GetMessageTypeSetWindow(this);
            if (valid.MessageType == "T")
            {
                senderTxtBox.Text = "@";
            }
        }

        private void MessageTypeTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            valid.CheckIsNumeric(e);
        }
        private void SenderTypeTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (valid.MessageType == "S")
            {
                valid.CheckIsNumeric(e);
            }
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            // Write the string to a file.
            string line = string.Format("{0},{1},{2},{3},{4}", messageTypeComboBox.Text, messageTypeTxtBox.Text, senderTxtBox.Text, subjectTxtBox.Text, messageTxtBox.Text);
            StreamWriter file = new StreamWriter(@"Message.csv", true);
            file.WriteLine(line);
            file.Close();
            messageTxtBox.Clear();
            senderTxtBox.Clear();
            subjectTxtBox.Clear();
            messageTxtBox.Clear();
            messageTypeTxtBox.Clear();
            messageTypeComboBox.SelectedIndex = -1;
            //changes create buuton text to start

        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var reader = new StreamReader(@"Message.csv"))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    messageTypeComboBox.Text = values[0];
                    messageTypeTxtBox.Text = values[1];
                    senderTxtBox.Text = values[2];
                    subjectTxtBox.Text = values[3];
                    messageTxtBox.Text = values[4];

                    if (messageTypeComboBox.Text == "S")
                    {
                        MessageBox.Show("SMS Loaded");
                    }
                    else if (messageTypeComboBox.Text == "E")
                    {
                        MessageBox.Show("E-Mail Loaded");
                    }
                    else if (messageTypeComboBox.Text == "T")
                    {
                        MessageBox.Show("Tweet Loaded");
                    }
                }
            }
        }

    }
}






