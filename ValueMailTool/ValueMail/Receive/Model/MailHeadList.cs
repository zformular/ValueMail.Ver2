using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ValueMail.Receive.Model
{
    public class MailHeadList : ReadOnlyCollection<MailHeadModel>
    {
        public MailHeadList(MailBase.MailBase mailBase)
            : base(new List<MailHeadModel>())
        {
            List<MailHeadModel> headerList = mailBase.GetMailHeaders();
            foreach (MailHeadModel header in headerList)
            {
                base.Items.Add(header);
            }
        }

        public MailHeadList(MailBase.MailBase mailBase, String expression)
            : base(new List<MailHeadModel>())
        {
            List<MailHeadModel> headerList = mailBase.GetMailHeaders(expression);
            foreach (MailHeadModel header in headerList)
            {
                base.Items.Add(header);
            }
        }
    }
}
