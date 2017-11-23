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
        List<Message> tweetList = new List<Message>();
        List<Message> smsList = new List<Message>();
        List<Message> emailList = new List<Message>();
        List<Message> SirEmailList = new List<Message>();




        public CreateMessage()
        {
            InitializeComponent();
            //hides Ui elements at start-up
            subjectLbl.Visibility = Visibility.Hidden;
            subjectTxtBox.Visibility = Visibility.Hidden;
            twitter.Visibility = Visibility.Hidden;
            subjectDate.Visibility = Visibility.Hidden;
            SirInfoBlock.Visibility = Visibility.Hidden;
            cancelBtn.Visibility = Visibility.Hidden;
            DomainExists();
        }

        #region createLoadMessage event handlers

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
            //gets message type and sets the forms depending on message type
            valid.GetMessageTypeSetWindow(this);
        }

        private void MessageTypeTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //checks to make sure only numbers are entered into textbox
            valid.CheckIsNumeric(e);
        }

        private void SenderTypeTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //checks to make sure only numbers are entered into textbox
            if (valid.MessageType == "S")
            {
                valid.CheckIsNumeric(e);
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            //validation for create form
            if (messageTypeComboBox.Text.Length <= 0 || messageTypeTxtBox.Text.Length < 9 || senderTxtBox.Text.Length <= 0 || messageTxtBox.Text.Length <= 0)
            {
                MessageBox.Show("your must fill in all setions of the form to continue");
            }
            else
            {
                string coverted = subjectDate.SelectedDate.Value.Date.ToShortDateString();

                if (subjectTxtBox.Text == "SIR" || subjectTxtBox.Text == "sir")
                {
                    // Write the string to a file.
                    string line = string.Format("{0},{1},{2},{3},{4}", messageTypeComboBox.Text, messageTypeTxtBox.Text, senderTxtBox.Text, subjectTxtBox.Text.ToUpper() + coverted, messageTxtBox.Text);
                    StreamWriter file = new StreamWriter(@"Message.csv", true);
                    file.WriteLine(line);
                    file.Close();
                }
                else
                {
                    // Write the string to a file.
                    string line = string.Format("{0},{1},{2},{3},{4}", messageTypeComboBox.Text, messageTypeTxtBox.Text, senderTxtBox.Text, subjectTxtBox.Text, messageTxtBox.Text);
                    StreamWriter file = new StreamWriter(@"Message.csv", true);
                    file.WriteLine(line);
                    file.Close();
                }

            }
            //resets the create/load message form
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
            SirEmailList.Clear();
            tweetList.Clear();
            valid.MentionsList.Clear();
            valid.TrendingList.Clear();
            valid.SirList.Clear();
            valid.IncidentList.Clear();

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
                        string subject = values[3];

                        if (subject[0] == 'S' && subject[1] == 'I' && subject[2] == 'R' || subject[0] == 's' && subject[1] == 'i' && subject[2] == 'r')
                        {
                            SirEmailMessage message = new SirEmailMessage
                            {
                                MessageId = values[0] + values[1],
                                SenderTxt = values[2],
                                Subject = values[3],
                                MessageTxt = values[4]
                            };
                            SirEmailList.Add(message);
                        }
                        else
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
                sirEmailBox.ItemsSource = SirEmailList;

                foreach (var item in tweetList)
                {
                    valid.CheckForTweetName(item, this);
                    valid.CheckForTweetTrend(item, this);
                }

                foreach (var item in SirEmailList)
                {
                    valid.CheckForSirCentre(item, this);
                }
                foreach (var item in SirEmailList)
                {
                    valid.CheckForSirType(item, this);
                }

            }
        }

        private void SmsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                createBtn.Visibility = Visibility.Hidden;
                ClearValues();
                var sms = ((SmsMessage)SmsListBox.SelectedItem);
                messageTypeComboBox.Text = sms.MessageId[0].ToString();
                messageTypeTxtBox.Text = ((SmsMessage)SmsListBox.SelectedItem).MessageId;
                senderTxtBox.Text = ((SmsMessage)SmsListBox.SelectedItem).SenderTxt;
                messageTxtBox.Text = ((SmsMessage)SmsListBox.SelectedItem).MessageTxt;
                valid.CheckForSpeak(sms, this);
            }
            catch
            {

            }


        }

        private void EMailListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                createBtn.Visibility = Visibility.Hidden;
                ClearValues();
                var eMail = ((EmailMessage)EMailListBox.SelectedItem);
                messageTypeComboBox.Text = eMail.MessageId[0].ToString();
                messageTypeTxtBox.Text = ((EmailMessage)EMailListBox.SelectedItem).MessageId;
                senderTxtBox.Text = ((EmailMessage)EMailListBox.SelectedItem).SenderTxt;
                subjectTxtBox.Text = ((EmailMessage)EMailListBox.SelectedItem).Subject;
                messageTxtBox.Text = ((EmailMessage)EMailListBox.SelectedItem).MessageTxt;
                valid.CheckForURL(eMail, this);
            }
            catch
            {

            }

        }

        private void TweetListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                createBtn.Visibility = Visibility.Hidden;
                ClearValues();
                var tweet = ((TweetMessage)TweetListBox.SelectedItem);
                messageTypeComboBox.Text = tweet.MessageId[0].ToString();
                messageTypeTxtBox.Text = ((TweetMessage)TweetListBox.SelectedItem).MessageId;
                senderTxtBox.Text = ((TweetMessage)TweetListBox.SelectedItem).SenderTxt;
                messageTxtBox.Text = ((TweetMessage)TweetListBox.SelectedItem).MessageTxt;
                valid.CheckForSpeak(tweet, this);
                valid.CheckForTweetName(tweet, this);
            }
            catch
            {

            }

        }

        private void SirEmailBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                createBtn.Visibility = Visibility.Hidden;
                ClearValues();
                var eMail = ((SirEmailMessage)sirEmailBox.SelectedItem);
                messageTypeComboBox.Text = eMail.MessageId[0].ToString();
                messageTypeTxtBox.Text = ((SirEmailMessage)sirEmailBox.SelectedItem).MessageId;
                senderTxtBox.Text = ((SirEmailMessage)sirEmailBox.SelectedItem).SenderTxt;
                subjectTxtBox.Text = ((SirEmailMessage)sirEmailBox.SelectedItem).Subject;
                messageTxtBox.Text = ((SirEmailMessage)sirEmailBox.SelectedItem).MessageTxt;
                valid.CheckForURL(eMail, this);
            }
            catch
            {

            }
        }

        private void MessageTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            valid.GetMessageTypeSetWindow(this);
            createBtn.Visibility = Visibility.Visible;
        }

        private void SubjectTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (subjectTxtBox.Text.ToUpper() == "SIR")
            {
                subjectDate.Visibility = Visibility.Visible;
                messageTxtBox.Text = "Sport Centre Code:[00-000-00]                                                                                           Nature of Incident:[Staff Attack]";
            }


        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            subjectDate.Visibility = Visibility.Hidden;
            cancelBtn.Visibility = Visibility.Hidden;
            SirInfoBlock.Visibility = Visibility.Hidden;
            createBtn.Visibility = Visibility.Hidden;
            ClearValues();
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Message> messageList = new List<Message>();

            foreach (var item in smsList)
            {
                messageList.Add(item);
            }
            foreach (var item in emailList)
            {
                messageList.Add(item);
            }
            foreach (var item in SirEmailList)
            {
                messageList.Add(item);
            }
            foreach (var item in tweetList)
            {
                messageList.Add(item);
            }
            String json = JsonConvert.SerializeObject(messageList, Formatting.Indented);
            System.IO.File.WriteAllText(@"JsonMessage.Json", json);
        }

        #endregion

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
                        valid.Textwords.Add(line[0], line[1]);
                    }
                    catch (Exception e)
                    {


                    }

                }
            }
        }

        private void ClearValues()
        {
            messageTypeComboBox.Text = "";
            messageTypeTxtBox.Text = "";
            senderTxtBox.Text = "";
            messageTxtBox.Text = "";
            QuarantinedBox.ItemsSource = "";
        }




    }


}











