using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EustonLeisureMessageFilter
{
    class Validation
    {
        private string messageType;
        private bool isEmailValid;
        private bool isNumlValid;
        private List<string> mentionsList = new List<string>();
        private List<string> trendingList = new List<string>();
        private List<string> sirList = new List<string>();
        private List<string> incidentList = new List<string>();
        private Dictionary<string, string> textwords = new Dictionary<string, string>();

        //constructor
        public Validation()
        {
            messageType = MessageType;
            isEmailValid = IsEmailValid;
            isNumlValid = IsNumlValid;
            mentionsList = MentionsList;
            trendingList = TrendingList;
            sirList = SirList;
            incidentList = IncidentList;
            textwords = Textwords;
        }

        public string MessageType
        {
            get
            {
                return messageType;
            }
            set
            {
                messageType = value;
            }
        }
        public bool IsEmailValid
        {
            get
            {
                return isEmailValid;
            }
            set
            {
                isEmailValid = value;
            }
        }
        public bool IsNumlValid
        {
            get
            {
                return isNumlValid;
            }
            set
            {
                isNumlValid = value;
            }
        }
        public List<string> MentionsList
        {
            get
            {
                return mentionsList;
            }
            set
            {
                mentionsList = value;
            }
        }
        public List<string> TrendingList
        {
            get
            {
                return trendingList;
            }
            set
            {
                trendingList = value;
            }
        }
        public List<string> SirList
        {
            get
            {
                return sirList;
            }
            set
            {
                sirList = value;
            }
        }
        public List<string> IncidentList
        {
            get
            {
                return incidentList;
            }
            set
            {
                incidentList = value;
            }
        }
        public Dictionary<string, string> Textwords
        {
            get
            {
                return textwords;
            }
            set
            {
                textwords = value;
            }
        }

        public void GetMessageTypeSetWindow(CreateMessage window)
        {

            window.senderTxtBox.Clear();
            window.messageTypeTxtBox.Clear();
            window.subjectTxtBox.Clear();
            if (window.subjectTxtBox.Text == "SIR" && window.messageTypeComboBox.Text.ToString() == "sir")
            {
                //shows subject label and textbox
                window.subjectLbl.Visibility = Visibility.Visible;
                window.subjectDate.Visibility = Visibility.Visible;
                window.subjectTxtBox.Visibility = Visibility.Visible;
                window.twitter.Visibility = Visibility.Hidden;
                window.cancelBtn.Visibility = Visibility.Visible;
                window.subjectTxtBox.MaxLength = 20;
                window.messageTxtBox.MaxLength = 1028;
                window.senderTxtBox.MaxLength = 254;
                //gets the message type
                messageType = "E";
            }
            //what the comboBox has been set to 
            else if (window.messageTypeComboBox.Text.ToString() == "E")
            {
                //shows subject label and textbox
                window.subjectLbl.Visibility = Visibility.Visible;
                window.subjectDate.Visibility = Visibility.Hidden;
                window.subjectTxtBox.Visibility = Visibility.Visible;
                window.twitter.Visibility = Visibility.Hidden;
                window.SirInfoBlock.Visibility = Visibility.Visible;
                window.cancelBtn.Visibility = Visibility.Visible;
                window.subjectTxtBox.MaxLength = 20;
                window.messageTxtBox.MaxLength = 1028;
                window.senderTxtBox.MaxLength = 254;
                //gets the message type
                messageType = "E";

            }
            else if (window.messageTypeComboBox.Text.ToString() == "S")
            {
                //hides subject label and textbox
                window.subjectLbl.Visibility = Visibility.Hidden;
                window.subjectDate.Visibility = Visibility.Hidden;
                window.subjectTxtBox.Visibility = Visibility.Hidden;
                window.twitter.Visibility = Visibility.Hidden;
                window.SirInfoBlock.Visibility = Visibility.Hidden;
                window.cancelBtn.Visibility = Visibility.Hidden;
                window.messageTxtBox.MaxLength = 140;
                window.senderTxtBox.MaxLength = 15;
                //gets the message type
                messageType = "S";
            }
            else if (window.messageTypeComboBox.Text.ToString() == "T")
            {
                //hides subject label and textbox
                window.subjectLbl.Visibility = Visibility.Hidden;
                window.subjectDate.Visibility = Visibility.Hidden;
                window.subjectTxtBox.Visibility = Visibility.Hidden;
                window.twitter.Visibility = Visibility.Visible;
                window.SirInfoBlock.Visibility = Visibility.Hidden;
                window.cancelBtn.Visibility = Visibility.Hidden;
                window.messageTxtBox.MaxLength = 140;
                window.senderTxtBox.MaxLength = 16;
                //gets the message type
                messageType = "T";
            }

        }

        public void CheckIsNumeric(TextCompositionEventArgs e)
        {
            //checks if input is number
            if (!(int.TryParse(e.Text, out int result) || e.Text == "."))
            {
                e.Handled = true;
            }
        }

        public void CheckNumber(string phoneNumber)
        {
            string MatchPhoneNumberPattern = "^/(?([0-9]{3})/)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (phoneNumber != null)
            {
                isNumlValid = Regex.IsMatch(phoneNumber, MatchPhoneNumberPattern);
            }
            else
            {
                isNumlValid = false;
                MessageBox.Show("A valid phone number must be entered");
            }
        }

        public void CheckEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                isEmailValid = true;
            }
            catch (FormatException)
            {
                isEmailValid = false;
            }
        }

        public void CheckForSpeak(Message message, CreateMessage window)
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
            window.messageTxtBox.Text = sb.ToString();
        }

        public void CheckForTweetName(Message message, CreateMessage window)
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

            window.MentionBox.ItemsSource = sortedDict;

        }

        public void CheckForTweetTrend(Message message, CreateMessage window)
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

            window.trendingBox.ItemsSource = sortedDict;

        }

        public void CheckForURL(Message message, CreateMessage window)
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

            window.messageTxtBox.Text = replaced;
            window.QuarantinedBox.ItemsSource = quarantineList;

        }

        public void CheckForSirType(Message message, CreateMessage window)
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

            window.incidentListBox.ItemsSource = sortedDict;

        }

        public void CheckForSirCentre(Message message, CreateMessage window)
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


            window.reportListBox.ItemsSource = sortedDict;

        }
    }
}
