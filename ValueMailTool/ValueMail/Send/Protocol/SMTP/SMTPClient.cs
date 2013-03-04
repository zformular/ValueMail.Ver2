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
using System.ComponentModel;
using ValueMail.Send.Infrastructure;
using ValueMail.Receive.Infrastructure;
using ValueMail.Model;

namespace ValueMail.Send.SMTP
{
    public class SMTPClient : IDisposable
    {
        private SmtpClient client = null;
        private SmtpModel smtpModel = null;

        private String account = null;
        private String password = null;
        private String displayName = null;

        public SMTPClient()
        {
            client = new System.Net.Mail.SmtpClient();
        }

        public void Connect(SmtpModel model)
        {
            this.smtpModel = model;
        }

        public void Loging(String mailAccount, String password, String displayName)
        {
            this.displayName = displayName;
            this.account = mailAccount;
            this.password = password;
        }

        /// <summary>
        ///  发送邮件
        /// </summary>
        /// <param name="sendModel"></param>
        public void SendMail(SendModel sendModel)
        {
            if (sendModel == null)
                throw new ArgumentException("请设置发送消息内容 调用方法SetSendInfo");

            var msg = SendHelper.PacketMessage(sendModel);
            if (!String.IsNullOrEmpty(displayName))
                msg.From = new MailAddress(account, displayName);
            else
                msg.From = new MailAddress(account);

            client.Credentials = new System.Net.NetworkCredential(this.account, this.password);
            client.Port = smtpModel.Port;
            client.Host = smtpModel.Host;
            client.EnableSsl = smtpModel.Ssl;

            try
            {
                client.Send(msg);
            }
            catch (System.Net.Mail.SmtpException sex)
            {
                throw (sex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private Boolean disposed = false;
        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    client = null;
                    smtpModel = null;
                    account = null;
                    password = null;
                    displayName = null;
                    disposed = true;
                }
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 析构函数

        ~SMTPClient()
        {
            Dispose(false);
        }

        #endregion

    }
}
