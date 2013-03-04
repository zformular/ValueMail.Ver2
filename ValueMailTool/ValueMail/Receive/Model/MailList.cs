using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ValueMail.Receive.Model
{
    public class MailList : ReadOnlyCollection<MailModel>
    {
        public MailList(MailBase.MailBase mailBase)
            : base(new List<MailModel>())
        {
            List<MailModel> mailList = mailBase.GetMails();
            foreach (MailModel mail in mailList)
            {
                base.Items.Add(mail);
            }
        }
    }
}
