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
        List<string> mentionsList = new List<string>();
        List<string> trendingList = new List<string>();
        List<string> sirList = new List<string>();
        List<string> incidentList = new List<string>();


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
            createBtn.Visibility = Visibility.Hidden;
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

        private void createBtn_Click(object sender, RoutedEventArgs e)
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
            createBtn.Visibility = Visibility.Hidden;

            List<Message> tweetList = new List<Message>();
            List<Message> smsList = new List<Message>();
            List<Message> emailList = new List<Message>();
            List<Message> SirEmailList = new List<Message>();

            trendingList.Clear();
            mentionsList.Clear();
            smsList.Clear();
            emailList.Clear();
            SirEmailList.Clear();
            tweetList.Clear();
            mentionsList.Clear();
            trendingList.Clear();
            sirList.Clear();
            incidentList.Clear();

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
                    CheckForTweetName(item);
                    CheckForTweetTrend(item);
                }

                foreach (var item in SirEmailList)
                {
                    CheckForSirCentre(item);
                }
                foreach (var item in SirEmailList)
                {
                    CheckForSirType(item);
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
                CheckForSpeak(sms);
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
                CheckForURL(eMail);
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
                CheckForSpeak(tweet);
                CheckForTweetName(tweet);
            }
            catch
            {

            }

        }

        private void sirEmailBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                CheckForURL(eMail);
            }
            catch
            {

            }
        }

        private void messageTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            valid.GetMessageTypeSetWindow(this);
            createBtn.Visibility = Visibility.Visible;
        }

        private void subjectTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (subjectTxtBox.Text.ToUpper() == "SIR")
            {
                subjectDate.Visibility = Visibility.Visible;
                messageTxtBox.Text = "Sport Centre Code:[00-000-00]                                                                                           Nature of Incident:[Staff Attack]";
            }


        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            subjectDate.Visibility = Visibility.Hidden;
            cancelBtn.Visibility = Visibility.Hidden;
            SirInfoBlock.Visibility = Visibility.Hidden;
            createBtn.Visibility = Visibility.Hidden;
            ClearValues();
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
                        textwords.Add(line[0], line[1]);
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

        private void CheckForSpeak(Message message)
        {
            StringBuilder sb = new StringBuilder();
            var line = message.MessageTxt;
            var splits = line.Split(' ');

            foreach (var item in splits)
            {
                if (textwords.ContainsKey(item.ToUpper()))
                {
                    string converted = textwords[item.ToUpper()];
                    sb.Append(item + " <" + converted + "> ");
                }
                else
                {
                    sb.Append(item + " ");
                }
            }
            messageTxtBox.Text = sb.ToString();
        }

        private void CheckForTweetName(Message message)
        {



            var line = message.MessageTxt;
            var splits = line.Split(' ');

            foreach (var word in splits)
            {
                try
                {
                    if (word[0] == '@')
                    {
                        mentionsList.Add(word);
                    }
                }
                catch
                {

                }

            }

            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (string word in mentionsList)
            {
                if (dictionary.ContainsKey(word))
                {
                    dictionary[word] += 1;
                }
                else
                {
                    dictionary.Add(word, 1);
                }
            }
            var sortedDict = from entry in dictionary orderby entry.Value descending select entry;

            MentionBox.ItemsSource = sortedDict;

        }

        private void CheckForTweetTrend(Message message)
        {
            //create dictionary to find distict values and store how many time they repeat
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            //takes message class messageTxt
            var line = message.MessageTxt;
            //splits in words
            var splits = line.Split(' ');

            //if word has # at start add to list
            foreach (var word in splits)
            {
                if (word == "")
                {

                }
                else
                {
                    if (word[0] == '#')
                    {
                        trendingList.Add(word);
                    }
                }
            }

            foreach (string word in trendingList)
            {
                if (dictionary.ContainsKey(word))
                {
                    dictionary[word] += 1;
                }
                else
                {
                    dictionary.Add(word, 1);
                }
            }
            var sortedDict = from entry in dictionary orderby entry.Value descending select entry;

            trendingBox.ItemsSource = sortedDict;

        }

        private void CheckForURL(Message message)
        {
            List<string> quarantineList = new List<string>();

            string replaced = null;

            var line = message.MessageTxt;
            var splits = line.Split(' ');

            foreach (var item in splits)
            {
                replaced = Regex.Replace(line, @"((http|ftp|https):\/\/)?(([\w.-]*)\.([\w]*))", "<quar>");
                Regex regex = new Regex(@"((http|ftp|https):\/\/)?(([\w.-]*)\.([\w]*))");
                if (regex.IsMatch(item))
                    quarantineList.Add(item);

            }

            messageTxtBox.Text = replaced;
            QuarantinedBox.ItemsSource = quarantineList;

        }

        private void CheckForSirType(Message message)
        {

            //create dictionary to find distict values and store how many time they repeat
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            List<string> natureOfIncident = new List<string>();

            if (natureOfIncident.Count <= 0)
            {
                natureOfIncident.Add("Theft of Properties");
                natureOfIncident.Add("Staff Attack");
                natureOfIncident.Add("Device Damage");
                natureOfIncident.Add("Raid");
                natureOfIncident.Add("Customer Attack");
                natureOfIncident.Add("Staff Abuse");
                natureOfIncident.Add("Bomb Threat");
                natureOfIncident.Add("Terrorism");
                natureOfIncident.Add("Suspicious Incident");
                natureOfIncident.Add("Sport Injury");
                natureOfIncident.Add("Personal info Leak");
            }


            //takes message class messageTxt
            var line = message.MessageTxt;
            //splits in words
            var splits = line.Split('[', ']');

            //if word has match regex 
            foreach (var word in splits)
            {
                if (natureOfIncident.Contains(word))
                {
                    incidentList.Add(word);
                }
            }

            foreach (string word in incidentList)
            {
                if (dictionary.ContainsKey(word))
                {
                    dictionary[word] += 1;
                }
                else
                {
                    dictionary.Add(word, 1);
                }
            }
            var sortedDict = from entry in dictionary orderby entry.Value descending select entry;


            incidentListBox.ItemsSource = sortedDict;

        }

        private void CheckForSirCentre(Message message)
        {
            //create dictionary to find distict values and store how many time they repeat
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            //takes message class messageTxt
            var line = message.MessageTxt;
            //splits in words
            var splits = line.Split('[', ']');

            Regex regex = new Regex(@"^[0-9]{2}[-][0-9]{3}[-][0-9]{2}$");

            //if word has match regex 
            foreach (var word in splits)
            {
                Match match = regex.Match(word);
                if (match.Success)
                {
                    sirList.Add(word);
                }

            }

            string code = "Num: ";

            foreach (string word in sirList)
            {
                if (dictionary.ContainsKey(code + word))
                {
                    dictionary[code + word] += 1;
                }
                else
                {
                    dictionary.Add(code + word, 1);
                }
            }
            var sortedDict = from entry in dictionary orderby entry.Value descending select entry;


            reportListBox.ItemsSource = sortedDict;

        }

    }


}











