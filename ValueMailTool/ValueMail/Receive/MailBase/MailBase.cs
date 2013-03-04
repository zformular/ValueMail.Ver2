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
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ValueMail.Receive.Model;
using ValueMail.Receive.Infrastructure;
using ValueMail.Receive.IMAP.Infrastructure;

namespace ValueMail.Receive.MailBase
{
    public class MailBase : IMailBase
    {
        /// <summary>
        ///  连接服务的对象
        /// </summary>
        protected TcpClient client = null;

        /// <summary>
        ///  读取字节的对象
        /// </summary>
        protected StreamReader streamReader = null;

        /// <summary>
        ///  写入字节的对象
        /// </summary>
        protected StreamWriter streamWriter = null;

        /// <summary>
        ///  邮箱供应商类型
        /// </summary>
        protected ProviderType providerType;

        /// <summary>
        ///  记录服务器上返回的数据
        /// </summary>
        protected String response;

        /// <summary>
        ///  数据流对象
        /// </summary>
        private Stream stream = null;

        /// <summary>
        ///  邮箱是否支持ssl加密
        /// </summary>
        protected Boolean ssl = false;

        /// <summary>
        ///  连接服务器
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public virtual void Connect(string server, int port)
        {
            Connect(server, port, false);
        }

        /// <summary>
        ///  连接服务器
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        public virtual void Connect(string server, int port, bool ssl)
        {
            try
            {
                this.ssl = ssl;
                client = new TcpClient(server, port);
                client.SendTimeout = 5000;
                client.ReceiveTimeout = 5000;
                if (ssl)
                {
                    SslStream sslStream = new SslStream(client.GetStream(), false);
                    sslStream.AuthenticateAsClient(server);
                    stream = sslStream;
                }
                else
                {
                    NetworkStream netStream = client.GetStream();
                    stream = netStream;
                }
                stream.ReadTimeout = 5000;
                streamReader = new StreamReader(stream, System.Text.Encoding.Default);
                streamWriter = new StreamWriter(stream, System.Text.Encoding.Default);
                setProvider(server);
            }
            catch (Exception)
            {
                throw new ArgumentException("服务器连接失败.");
            }
        }

        public virtual Boolean Loging(string account, string password)
        {
            throw new NotImplementedException();
        }

        public virtual List<MailHeadModel> GetMailHeaders()
        {
            throw new NotImplementedException();
        }

        public virtual List<MailHeadModel> GetMailHeaders(SearchType searchType)
        {
            throw new NotImplementedException();
        }

        public virtual List<MailHeadModel> GetMailHeaders(String expression)
        {
            throw new NotImplementedException();
        }

        public virtual MailModel GetMail(Int32 index)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleMail(Int32 index)
        {
            throw new NotImplementedException();
        }

        public virtual List<MailModel> GetMails()
        {
            throw new NotImplementedException();
        }

        public virtual void ResetStatus()
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            Dispose(true);
        }

        /// <summary>
        ///  设置邮箱供应商类型
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        private void setProvider(String server)
        {
            String provider = Regex.Match(server, @"[.]\w*[.]").Value.ToLower();
            switch (provider)
            {
                case ".gmail.":
                    providerType = ProviderType.谷歌;
                    break;
                case ".163.":
                    providerType = ProviderType.网易163;
                    break;
                case ".126.":
                    providerType = ProviderType.网易126;
                    break;
                case ".sina.":
                    providerType = ProviderType.新浪;
                    break;
                case ".qq.":
                    providerType = ProviderType.腾讯;
                    break;
                default:
                    providerType = ProviderType.Value;
                    break;
            }
        }

        /// <summary>
        ///  标定是否已经释放过资源
        /// </summary>
        private Boolean disposed;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (client != null)
                    {
                        client.Close();
                        client = null;
                    }

                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                        stream = null;
                    }

                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                        streamWriter = null;
                    }

                    if (streamReader != null)
                    {
                        streamReader.Close();
                        streamReader = null;
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

        ~MailBase()
        {
            Dispose(false);
        }

        #endregion
    }
}
