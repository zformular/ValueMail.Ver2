using System;
using ValueMail.Receive.POP3;
using ValueMail.Receive.IMAP;
using ValueMail.Send.SMTP;

namespace ValueMail
{
    public class ClientManager
    {
        /// <summary>
        ///  获得POP3协议对象 并登陆
        /// </summary>
        /// <param name="mailAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public POP3Client GetPop3Client(String mailAccount, String password)
        {
            var host = MailHelper.GetHost(mailAccount).PopHost;
            if (host == null)
                throw new ArgumentException("项目配置不支持当前邮箱服务器");

            POP3Client client = new POP3Client();
            client.Connect(host.Host, host.Port, host.Ssl);
            client.Loging(mailAccount, password);

            return client;
        }

        /// <summary>
        ///  获得IMAP协议对象 并登陆
        /// </summary>
        /// <param name="mailAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IMAPClient GetImapClient(String mailAccount, String password)
        {
            var host = MailHelper.GetHost(mailAccount).ImapHost;
            if (host == null)
                throw new ArgumentException("项目配置不支持当前邮箱服务器");

            IMAPClient client = new IMAPClient();
            client.Connect(host.Host, host.Port, host.Ssl);
            client.Login(mailAccount, password);

            return client;
        }

        /// <summary>
        ///  获得IMAP协议对象 并登陆
        /// </summary>
        /// <param name="mailAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SMTPClient GetSmtpClient(String mailAccount, String password)
        {
            return this.GetSmtpClient(mailAccount, password, null);
        }

        /// <summary>
        ///  获得SMTP协议对象 并登陆
        /// </summary>
        /// <param name="mailAccount"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public SMTPClient GetSmtpClient(String mailAccount, String password, String displayName)
        {
            var host = MailHelper.GetHost(mailAccount).SmtpHost;
            if (host == null)
                throw new ArgumentException("项目配置不支持当前邮箱服务器");

            SMTPClient client = new SMTPClient();
            client.Connect(host);
            client.Loging(mailAccount, password, displayName);

            return client;
        }
    }
}
