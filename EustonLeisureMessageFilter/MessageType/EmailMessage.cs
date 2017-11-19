using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EustonLeisureMessageFilter.MessageType
{
    class EmailMessage : Message
    {
        private string subject;

        public EmailMessage()
        {
            subject = Subject;
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
