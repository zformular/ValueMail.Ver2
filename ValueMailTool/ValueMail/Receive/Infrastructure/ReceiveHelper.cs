/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 9.27.2012              |
 |                                            |
 ╰==========================================╯
 
 */

using System;
using System.Text.RegularExpressions;
using ValueHelper.OtherHelper;
using ValueMail.Receive.Model;
using ValueHelper.MIMEHelper;
using ValueHelper.MIMEHelper.Infrastructure;

namespace ValueMail.Receive.Infrastructure
{
    public class ReceiveHelper
    {
        private static String getName(String From)
        {
            return From.Split(' ')[0];
        }

        private static String getAddress(String From)
        {
            return Regex.Match(From, @"\<(?<address>.*)\>").Groups["address"].Value;
        }

        private static DateTime getDate(String date)
        {
            var set = date.Split(' ');
            var groups = Regex.Match(date, @"(?<day>\d{1,2})\s(?<month>\w{3})\s(?<year>\d{4})\s(?<time>\d{2}:\d{2}:\d{2})").Groups;
            var month = DateHelper.ConvertEnToNum(groups["month"].Value);
            return DateTime.Parse(String.Concat(groups["year"], "/", month, "/", groups["day"], " ", groups["time"]));
        }

        public static MailHeadModel GetMailHead(String uid, String mimeHead)
        {
            var headSet = ValueMIME.SerializeMIMEHead(mimeHead);
            MailHeadModel model = new MailHeadModel(
                uid,
                getName(headSet[MIMEPrefix.From]),
                getAddress(headSet[MIMEPrefix.From]),
                headSet[MIMEPrefix.Subject],
                getDate(headSet[MIMEPrefix.Date]),
                headSet[MIMEPrefix.ContentEncoding]
            );
            return model;
        }

        public static MailModel GetMail(String uid, String mailHead, String mailBody)
        {
            return GetMail(uid, String.Concat(mailHead, mailBody));
        }

        public static MailModel GetMail(String uid, String mime)
        {
            var modelSet = ValueMIME.SerializeMIME(mime);
            MailModel model = new MailModel(
                GetMailHead(uid, mime),
                modelSet[MIMEPrefix.BodyText],
                modelSet[MIMEPrefix.BodyHtml],
                modelSet.Attachments
                );
            return model;
        }
    }
}
