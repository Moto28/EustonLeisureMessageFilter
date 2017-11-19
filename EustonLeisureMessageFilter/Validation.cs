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

        //constructor
        public Validation()
        {
            messageType = MessageType;
            isEmailValid = IsEmailValid;
            isNumlValid = IsNumlValid;
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

        public void GetMessageTypeSetWindow(CreateMessage window)
        {

            window.senderTxtBox.Clear();
            window.messageTypeTxtBox.Clear();
            window.subjectTxtBox.Clear();


            //what the comboBox has been set to 
            if (window.messageTypeComboBox.Text.ToString() == "E")
            {
                //shows subject label and textbox
                window.subjectLbl.Visibility = Visibility.Visible;
                window.subjectTxtBox.Visibility = Visibility.Visible;
                window.twitter.Visibility = Visibility.Hidden;
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
                window.subjectTxtBox.Visibility = Visibility.Hidden;
                window.twitter.Visibility = Visibility.Hidden;
                window.messageTxtBox.MaxLength = 140;
                window.senderTxtBox.MaxLength = 15;
                //gets the message type
                messageType = "S";
            }
            else if (window.messageTypeComboBox.Text.ToString() == "T")
            {
                //hides subject label and textbox
                window.subjectLbl.Visibility = Visibility.Hidden;
                window.subjectTxtBox.Visibility = Visibility.Hidden;
                window.twitter.Visibility = Visibility.Visible;
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

        public void CheckForTextSpeak(List<Message> messages)
        {

        }
    }
}
