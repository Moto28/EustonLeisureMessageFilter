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
        private string subject;
        private string messageTxt;

        public Message()
        {
            messageId = MessageId;
            senderTxt = SenderTxt;
            subject = Subject;
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
