using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EustonLeisureMessageFilter
{
    class Message
    {
        private string messageId;
        private string senderTxt;
        private string messageTxt;
        private string subject;


        public Message()
        {
            subject = Subject;
            messageId = MessageId;
            senderTxt = SenderTxt;
            messageTxt = MessageTxt;
        }

        public string MessageId
        {
            get
            {
                return messageId;
            }
            set
            {
                messageId = value;
            }
        }
        public string MessageTxt
        {
            get
            {
                return messageTxt;
            }
            set
            {
                messageTxt = value;
            }
        }
        public string SenderTxt
        {
            get
            {
                return senderTxt;
            }
            set
            {
                senderTxt = value;
            }
        }

        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }
    }


}
