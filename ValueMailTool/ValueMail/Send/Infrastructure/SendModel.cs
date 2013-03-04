using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace ValueMail.Send.Infrastructure
{
    public class SendModel
    {
        public SendModel()
        {
            Priority = MailPriority.Normal;
            To = new MailAddressCollection();
        }

        /// <summary>
        ///  邮件编号
        /// </summary>
        public String UID { get; set; }

        /// <summary>
        ///  收件箱
        /// </summary>
        private MailAddressCollection to = null;
        public MailAddressCollection To
        {
            get
            {
                if (to == null) return new MailAddressCollection();
                return to;
            }

            private set { to = value; }
        }

        public void AddTo(String mailAddress)
        {
            this.AddTo(mailAddress, null);
        }

        public void AddTo(String mailAddress, String dislpayName)
        {
            if (!String.IsNullOrEmpty(dislpayName))
                to.Add(new MailAddress(mailAddress, dislpayName));
            else
                to.Add(new MailAddress(mailAddress));
        }

        /// <summary>
        ///  主题
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        ///  内容
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        ///  优先级
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        ///  附件
        /// </summary>
        private IList<Attachment> attachemnts;
        public IList<Attachment> Attachments
        {
            get
            {
                if (attachemnts == null)
                    attachemnts = new List<Attachment>();
                return attachemnts;
            }
            private set { attachemnts = value; }
        }

        public void AddAttachmend(String fileName)
        {
            Attachments.Add(new Attachment(fileName));
        }
    }
}
