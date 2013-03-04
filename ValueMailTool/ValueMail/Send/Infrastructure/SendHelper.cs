/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 2.4.2013               |
 |                                            |
 ╰==========================================╯
 
 */

using System;
using System.Net.Mail;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ValueMail.Send.Infrastructure;
using System.Xml.Linq;
using ValueMail.Model;

namespace ValueMail.Send
{
    public class SendHelper
    {
        /// <summary>
        ///  包装发送的邮件的相关信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MailMessage PacketMessage(SendModel model)
        {
            var mail = new MailMessage();
            mail.Subject = model.Subject;
            mail.Body = model.Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = model.Priority;
            if (model.To != null)
                foreach (var item in model.To)
                    mail.To.Add(item);
            if (model.Attachments != null)
                foreach (var item in model.Attachments)
                    mail.Attachments.Add(item);
            return mail;
        }

        /// <summary>
        ///  获得邮件服务器参数
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static HostModel GetHost(String address)
        {
            return MailHelper.GetHost(address).SmtpHost;
        }
    }
}
