using System;
using ValueMail.Receive.Model;
using ValueMail.Receive.POP3.MailBase;

namespace ValueMail.Receive.POP3
{
    public class POP3Client : IDisposable
    {
        private POP3Base pop3Base = new POP3Base();

        public void Connect(string server, Int32 port)
        {
            pop3Base.Connect(server, port);
        }

        public void Connect(string server, Int32 port, bool ssl)
        {
            pop3Base.Connect(server, port, ssl);
        }

        public void Loging(string account, string password)
        {
            pop3Base.Loging(account, password);
        }

        /// <summary>
        ///  获得邮件数量
        /// </summary>
        /// <returns></returns>
        public Int32 GetMailCount()
        {
            return pop3Base.GetMailCount();
        }

        /// <summary>
        ///  获得邮件头部
        /// </summary>
        /// <returns></returns>
        public MailHeadList GetMailHeaders()
        {
            return new MailHeadList(pop3Base);
        }

        /// <summary>
        ///  获得所有邮件
        /// </summary>
        /// <returns></returns>
        public MailList GetMails()
        {
            return new MailList(pop3Base);
        }

        /// <summary>
        ///  获得指定邮件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MailModel GetMail(Int32 index)
        {
            return pop3Base.GetMail(index);
        }

        /// <summary>
        ///  删除邮件
        /// </summary>
        /// <param name="index"></param>
        public void DeleMail(Int32 index)
        {
            pop3Base.DeleMail(index);
        }

        /// <summary>
        ///  撤销当前未保存的执行
        /// </summary>
        public void ResetStatus()
        {
            pop3Base.ResetStatus();
        }

        /// <summary>
        ///  断开连接
        /// </summary>
        public void Disconnect()
        {
            pop3Base.Disconnect();
        }

        private Boolean disposed = false;
        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (pop3Base != null)
                    {
                        pop3Base.Dispose();
                        pop3Base = null;
                    }

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

        ~POP3Client()
        {
            Dispose(false);
        }

        #endregion
    }
}
