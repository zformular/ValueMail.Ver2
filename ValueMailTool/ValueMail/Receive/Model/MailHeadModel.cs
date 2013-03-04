using System;
using System.Net.Mime;

namespace ValueMail.Receive.Model
{
    public class MailHeadModel
    {
        #region 邮件头属性

        public String UID { get; private set; }

        public String Name { get; private set; }

        public String Address { get; private set; }

        public String Subject { get; private set; }

        public DateTime Date { get; private set; }

        public ContentType ContentType { get; private set; }

        public String ContentTransferEncoding { get; private set; }

        #endregion

        public MailHeadModel(String uid, String name, String address, String subject, DateTime date, String contentTransferEncoding)
        {
            this.UID = uid;
            this.Name = name;
            this.Address = address;
            this.Subject = subject;
            this.Date = date;
            this.ContentTransferEncoding = contentTransferEncoding;
        }
    }
}
