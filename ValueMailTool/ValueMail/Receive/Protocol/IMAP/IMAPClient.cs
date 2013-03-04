using System;
using ValueMail.Receive.Model;
using ValueMail.Receive.IMAP.MailBase;
using ValueMail.Receive.IMAP.Infrastructure;

namespace ValueMail.Receive.IMAP
{
    public class IMAPClient : IDisposable
    {
        private IMAPBase imapBase = new IMAPBase();

        public void Connect(String server, Int32 port)
        {
            imapBase.Connect(server, port);
        }

        public void Connect(String server, Int32 port, Boolean ssl)
        {
            imapBase.Connect(server, port, ssl);
        }

        public void Login(String account, String password)
        {
            imapBase.Loging(account, password);
        }

        /// <summary>
        ///  获得邮箱所有文件夹
        /// </summary>
        /// <returns></returns>
        public String ListMailbox()
        {
            return imapBase.ListMailbox();
        }

        /// <summary>
        ///  获得收件箱文件夹
        /// </summary>
        public void SelectINBOX()
        {
            imapBase.SelectMailbox("INBOX");
        }

        public void SelectMailbox(String mailboxName)
        {
            imapBase.SelectMailbox(mailboxName);
        }

        /// <summary>
        ///  获得邮件头部
        /// </summary>
        /// <param name="searchType">邮件类型</param>
        /// <returns></returns>
        public MailHeadList GetMailHeads(SearchType searchType)
        {
            return new MailHeadList(imapBase, searchType.ToString());
        }

        /// <summary>
        ///  获得邮件头部
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public MailHeadList GetMailHeads(String expression)
        {
            return new MailHeadList(imapBase, expression);
        }

        /// <summary>
        ///  获得指定序列邮件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MailModel GetMail(Int32 index)
        {
            return imapBase.GetMail(index);
        }

        /// <summary>
        ///  获得指定UID邮件
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public MailModel GetMailByUID(String UID)
        {
            return imapBase.GetMailByUID(UID);
        }

        /// <summary>
        ///  彻底删除
        /// </summary>
        /// <param name="mailIndex"></param>
        public void NoBackupDelete(Int32 mailIndex)
        {
            imapBase.DeleMail(mailIndex);
        }

        /// <summary>
        ///  先备份到<mailBox>参数所指定文件夹
        ///  再删除邮件
        /// </summary>
        /// <param name="index"></param>
        /// <param name="mailbox"></param>
        public void DeleteMail(Int32 index, String mailbox)
        {
            imapBase.CopyMail(index, mailbox);
            imapBase.DeleMail(index);
        }

        /// <summary>
        ///  断开连接
        /// </summary>
        public void Disconnect()
        {
            imapBase.Disconnect();
        }

        private Boolean disposed = false;
        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (imapBase != null)
                    {
                        imapBase.Dispose();
                        imapBase = null;
                    }
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

        ~IMAPClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
