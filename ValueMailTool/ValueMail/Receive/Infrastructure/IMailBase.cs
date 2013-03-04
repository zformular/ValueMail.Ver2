using System;
using System.Collections.Generic;
using ValueMail.Receive.Model;
using ValueMail.Receive.IMAP.Infrastructure;

namespace ValueMail.Receive.Infrastructure
{
    public interface IMailBase : IDisposable
    {
        /// <summary>
        ///  连接服务器
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        void Connect(String server, Int32 port);
        /// <summary>
        ///  连接服务器重载
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        /// <returns></returns>
        void Connect(String server, Int32 port, Boolean ssl);
        /// <summary>
        ///  登陆账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        Boolean Loging(String account, String password);
        /// <summary>
        ///  获得邮件头
        /// </summary>
        /// <returns></returns>
        List<MailHeadModel> GetMailHeaders();
        List<MailHeadModel> GetMailHeaders(SearchType searchType);
        List<MailHeadModel> GetMailHeaders(String expression);
        /// <summary>
        ///  获得完整邮件
        /// </summary>
        /// <returns></returns>
        MailModel GetMail(Int32 index);
        /// <summary>
        ///  删除邮件
        /// </summary>
        /// <param name="index"></param>
        void DeleMail(Int32 index);
        /// <summary>
        ///  获得完整邮件列表
        /// </summary>
        /// <returns></returns>
        List<MailModel> GetMails();
        /// <summary>
        ///  重置状态
        /// </summary>
        void ResetStatus();
        /// <summary>
        ///  断开与服务器的连接
        /// </summary>
        void Disconnect();
        /// <summary>
        ///  释放资源
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(Boolean disposing);
    }
}
