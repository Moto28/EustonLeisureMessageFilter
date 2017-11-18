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
        List<Message> messageList = new List<Message>();
        int clickCount;

        public CreateMessage()
        {
            InitializeComponent();
            //hides Ui elements at start-up
            subjectLbl.Visibility = Visibility.Hidden;
            subjectTxtBox.Visibility = Visibility.Hidden;
            twitter.Visibility = Visibility.Hidden;
            //deactivates controls
            messageTypeComboBox.IsEnabled = false;
            messageTypeTxtBox.IsEnabled = false;
            senderTxtBox.IsEnabled = false;
            messageTxtBox.IsEnabled = false;
            createBtn.Content = "Start";

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
            if (clickCount == 0)
            {
                //changes create buuton text to start
                createBtn.Content = "Start";
                //turns ui controls on 
                messageTypeComboBox.IsEnabled = true;
                messageTypeTxtBox.IsEnabled = true;
                senderTxtBox.IsEnabled = true;
                messageTxtBox.IsEnabled = true;
                //sets click count
                clickCount = 1;
                MessageBox.Show("Click create to add your message");
            }
            if (clickCount == 1)
            {
                //changes create button text
                createBtn.Content = "Create";
                //creates new message obj
                Message message = new Message();

                //depending on the form type creates a json object
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
                messageList.Add(message);

                String json = JsonConvert.SerializeObject(messageList, Formatting.Indented);
                System.IO.File.WriteAllText(@"JsonMessage.Json", json);
                ////writes the json object to text file
                //StreamWriter write = new StreamWriter(@"JsonMessage.Json", true);
                //string json = JsonConvert.SerializeObject(message, Formatting.Indented);
                //write.WriteLine(json);
                //write.Close();
                //resets click count
                clickCount = 0;

                messageTxtBox.Clear();
                senderTxtBox.Clear();
                subjectTxtBox.Clear();
                messageTxtBox.Clear();
                messageTypeTxtBox.Clear();
                messageTypeComboBox.SelectedIndex = -1;
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader reader = new StreamReader(@"JsonMessage.Json"))
            {

                string json = reader.ReadToEnd();
                dynamic messages = JsonConvert.DeserializeObject(json);

            }
        }
    }
}





