/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 9.27.2012              |
 |                                            |
 ╰==========================================╯

 
 * POP3指令:
 * USER username    认证用户名
 * PASS password    认证密码   错误回送-ER ...... 
 * STAT             回送邮箱内邮件数 总字大小
 * LIST             成功回送邮件序列和字节大小
 * TOP 1            接收第1封邮件, 成功返回第1封邮件头
 * RETR 1           接收第1封邮件, 成功返回第1封邮件所有内容
 * DELE 1           删除第1封邮件, QUIT命令后才会真正删除
 * RSET             撤销所有的DELE命令
 * QUIT             如果server处于处理状态,则进入'更新'状态即保存修改
 * QUIT             如果server处于认可状态,则不进入'更新'状态
 */


using System;
using System.IO;
using ValueMail.Receive.Infrastructure;

namespace ValueMail.Receive.POP3.Instruction
{
    public class POP3Instruction : IDisposable
    {
        private ProviderType providerTpye;
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private String response;

        public POP3Instruction(ProviderType providerType, StreamWriter streamWriter, StreamReader streamReader)
        {
            this.providerTpye = providerType;
            this.streamWriter = streamWriter;
            this.streamReader = streamReader;
        }

        /// <summary>
        ///  USER指令输入账户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public String USERResponse(String account)
        {
            streamWriter.WriteLine("USER " + account);
            streamWriter.Flush();
            response = streamReader.ReadLine();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  PASS指令输入密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public String PASSResponse(String password)
        {
            streamWriter.WriteLine("PASS " + password);
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  STAT指令查看邮件数量
        /// </summary>
        /// <returns></returns>
        public String STATResponse()
        {
            streamWriter.WriteLine("STAT ");
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  UIDL指令获得邮件唯一码
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public String UIDLResponse(Int32 index)
        {
            streamWriter.WriteLine("UIDL " + index);
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  TOP指令查看邮件头部信息
        /// </summary>
        /// <param name="index"></param>
        public String TOPResponse(Int32 index)
        {
            streamWriter.WriteLine("TOP " + index + " 0");
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  RETR指令查看完整邮件
        /// </summary>
        /// <param name="index"></param>
        public String RETRResponse(Int32 index)
        {
            streamWriter.WriteLine("RETR " + index);
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  DELE指令删除指定邮件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public String DELEResponse(Int32 index)
        {
            streamWriter.WriteLine("DELE " + index);
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  RSET指令撤销所有的删除指令
        /// </summary>
        /// <returns></returns>
        public String RSETResponse()
        {
            streamWriter.WriteLine("RSET ");
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  '认证'状态,则退出. '处理'状态则更新
        /// </summary>
        /// <returns></returns>
        public String QUITResponse()
        {
            streamWriter.WriteLine("QUIT ");
            streamWriter.Flush();
            response = streamReader.ReadLine();
            return response;
        }

        /// <summary>
        ///  读取回送的整行数据
        /// </summary>
        /// <returns></returns>
        public String ReadResponse()
        {
            response = streamReader.ReadLine();
            return response;
        }

        private Boolean disposed = false;
        private void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 析构函数

        ~POP3Instruction()
        {
            Dispose(false);
        }

        #endregion
    }
}
