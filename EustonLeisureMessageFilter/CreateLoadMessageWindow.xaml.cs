using Microsoft.VisualBasic.FileIO;
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
using EustonLeisureMessageFilter.MessageType;
using System.Text.RegularExpressions;

namespace EustonLeisureMessageFilter
{

    /// <summary>
    /// Interaction logic for CreateMessage.xaml
    /// </summary>
    public partial class CreateMessage : Window
    {
        Validation valid = new Validation();
        Dictionary<string, string> textwords = new Dictionary<string, string>();
        List<Message> smsList = new List<Message>();
        List<Message> emailList = new List<Message>();
        List<Message> tweetList = new List<Message>();

        string result;

        public CreateMessage()
        {
            InitializeComponent();
            //hides Ui elements at start-up
            subjectLbl.Visibility = Visibility.Hidden;
            subjectTxtBox.Visibility = Visibility.Hidden;
            twitter.Visibility = Visibility.Hidden;
            DomainExists();
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

            if (messageTypeComboBox.Text.Length <= 0 || messageTypeTxtBox.Text.Length <= 0 || senderTxtBox.Text.Length <= 0 || messageTxtBox.Text.Length <= 0)
            {
                MessageBox.Show("your must fill in all setions of the form to continue");
            }
            else
            {
                // Write the string to a file.
                string line = string.Format("{0},{1},{2},{3},{4}", messageTypeComboBox.Text, messageTypeTxtBox.Text, senderTxtBox.Text, subjectTxtBox.Text, messageTxtBox.Text);
                StreamWriter file = new StreamWriter(@"Message.csv", true);
                file.WriteLine(line);
                file.Close();
                //changes create button text to start
            }
            messageTxtBox.Clear();
            senderTxtBox.Clear();
            subjectTxtBox.Clear();
            messageTxtBox.Clear();
            messageTypeTxtBox.Clear();
            messageTypeComboBox.SelectedIndex = -1;

        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {

            smsList.Clear();
            emailList.Clear();
            tweetList.Clear();

            using (var reader = new StreamReader(@"Message.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values[0] == "S")
                    {
                        SmsMessage message = new SmsMessage
                        {
                            MessageId = values[0] + values[1],
                            SenderTxt = values[2],
                            MessageTxt = values[4]
                        };
                        smsList.Add(message);
                    }
                    else if (values[0] == "E")
                    {
                        EmailMessage message = new EmailMessage
                        {
                            MessageId = values[0] + values[1],
                            SenderTxt = values[2],
                            Subject = values[3],
                            MessageTxt = values[4]
                        };
                        emailList.Add(message);
                    }
                    else if (values[0] == "T")
                    {
                        TweetMessage message = new TweetMessage
                        {
                            MessageId = values[0] + values[1],
                            SenderTxt = values[2],
                            MessageTxt = values[4]
                        };
                        tweetList.Add(message);
                    }
                }
                reader.Close();
                SmsListBox.ItemsSource = smsList;
                EMailListBox.ItemsSource = emailList;
                TweetListBox.ItemsSource = tweetList;
            }
        }


        private void DomainExists()
        {
            using (FileStream reader = File.OpenRead(@"textwords.csv")) // mind the encoding - UTF8
            using (TextFieldParser parser = new TextFieldParser(reader))
            {
                parser.TrimWhiteSpace = true; // if you want
                parser.Delimiters = new[] { "," };
                parser.HasFieldsEnclosedInQuotes = true;
                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] line = parser.ReadFields();
                        textwords.Add(line[0], line[1]);
                    }
                    catch (Exception e)
                    {


                    }

                }
            }
        }

        private void SmsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearValues();
            var sms = ((SmsMessage)SmsListBox.SelectedItem);
            messageTypeComboBox.Text = sms.MessageId[0].ToString();
            messageTypeTxtBox.Text = ((SmsMessage)SmsListBox.SelectedItem).MessageId;
            senderTxtBox.Text = ((SmsMessage)SmsListBox.SelectedItem).SenderTxt;
            messageTxtBox.Text = ((SmsMessage)SmsListBox.SelectedItem).MessageTxt;
            CheckForSpeak(sms);

        }

        private void EMailListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearValues();
            var eMail = ((EmailMessage)SmsListBox.SelectedItem);
            messageTypeComboBox.Text = eMail.MessageId[0].ToString();
            messageTypeTxtBox.Text = ((EmailMessage)SmsListBox.SelectedItem).MessageId;
            senderTxtBox.Text = ((EmailMessage)SmsListBox.SelectedItem).SenderTxt;
            messageTxtBox.Text = ((EmailMessage)SmsListBox.SelectedItem).MessageTxt; ;
        }

        private void TweetListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearValues();
            var tweet = ((TweetMessage)SmsListBox.SelectedItem);
            messageTypeComboBox.Text = tweet.MessageId[0].ToString();
            messageTypeTxtBox.Text = ((TweetMessage)SmsListBox.SelectedItem).MessageId;
            senderTxtBox.Text = ((TweetMessage)SmsListBox.SelectedItem).SenderTxt;
            messageTxtBox.Text = ((TweetMessage)SmsListBox.SelectedItem).MessageTxt;
            CheckForSpeak(tweet);
        }

        private void ClearValues()
        {
            messageTypeComboBox.Text = "";
            messageTypeTxtBox.Text = "";
            senderTxtBox.Text = "";
            messageTxtBox.Text = "";
        }

        private void CheckForSpeak(Message message)
        {
            using (var reader = new StreamReader(@"Message.csv"))
            {
                StringBuilder sb = new StringBuilder();
                var line = message.MessageTxt;
                var splits = line.Split(',');

                foreach (var item in splits)
                {
                    if (textwords.ContainsKey(item.ToUpper()))
                    {
                        string converted = textwords[item.ToUpper()];
                        //item.Replace(item, converted);
                        sb.Append(item + "<" + converted + ">");
                    }
                    else
                    {
                        sb.Append(item + " ");
                    }
                }
                messageTxtBox.Text = sb.ToString();
            }


        }

    }

}








