using Newtonsoft.Json;
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

        public CreateMessage()
        {
            InitializeComponent();
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
            Message message = new Message();

            MessageBox.Show(valid.MessageType);
            if (valid.MessageType == "E")
            {
                //checks e-mail
                valid.CheckEmail(senderTxtBox.Text);
                //creates varibles for adding to JSON file
                message.MessageId = messageTypeComboBox.Text + messageTypeTxtBox.Text;
                message.SenderTxt = senderTxtBox.Text;
                message.Subject = subjectTxtBox.Text;
                message.MessageTxt = messageTxtBox.Text;

            }
            if (valid.MessageType == "S")
            {
                valid.CheckNumber(senderTxtBox.Text);
                //creates varibles for adding to JSON file
                message.MessageId = messageTypeComboBox.Text + messageTypeTxtBox.Text;
                message.SenderTxt = senderTxtBox.Text;
                message.Subject = subjectTxtBox.Text;
                message.MessageTxt = messageTxtBox.Text;
            }
            if (valid.MessageType == "T")
            {
                //creates varibles for adding to JSON file
                message.MessageId = messageTypeComboBox.Text + messageTypeTxtBox.Text;
                message.SenderTxt = senderTxtBox.Text;
                message.Subject = subjectTxtBox.Text;
                message.MessageTxt = messageTxtBox.Text;
            }
            StreamWriter write = new StreamWriter(@"JsonMessage.txt", true);
            write.WriteLine(JsonConvert.SerializeObject(message, Formatting.Indented));
            write.Close();

        }
    }
}
