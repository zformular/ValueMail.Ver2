using System;
using System.Collections.Generic;
using ValueHelper.MIMEHelper.Infrastructure;

namespace ValueMail.Receive.Model
{
    public class MailModel
    {
        public String Body { get; private set; }

        public String BodyHtml { get; private set; }

        public MailHeadModel MailHead { get; private set; }

        public IList<Attachment> Attachments { get; private set; }

        public MailModel(MailHeadModel mailHead, String body, String bodyHtml, IList<Attachment> attachment)
        {
            this.MailHead = mailHead;
            this.Body = body;
            this.BodyHtml = bodyHtml;
            this.Attachments = attachment;
        }
    }
}
