/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 10.2.2012              |
 |                                            |
 ╰==========================================╯

 * IMAP指令: 注:前面的A**仅仅是为了区分数据区间用的没有一定要匹配
 * A01 LOGIN username password          认证用户
 * A02 LIST                             列出所有信箱列表
 * A03 SELECT                           选择邮箱没文件夹(收件箱,草稿箱...)
 * A04 SEARCH                           查找邮件
 * A05 FETCH                            返回邮件指定信息
 * A06 STORE                            设置邮件的标志
 * A07 COPY                             复制邮件到指定邮箱
 * A08 CLOSE                            退出选中的邮箱
 * A09 LOGOUT                           退出登录
 * A10 UID FETCH                        根据UID来获得邮件
 * A11 UID STORE                        根据UID设置邮件标记
 * 
 * A21 CAPABILITY                       返回服务器所支持的功能
 * A22 NOOP                             返回一个肯定响应,防止长时间处于不活动状态导致连接中断
 */


using System;
using System.IO;
using System.Text.RegularExpressions;
using ValueMail.Receive.Infrastructure;
using ValueMail.Receive.IMAP.Infrastructure;

namespace ValueMail.Receive.Instruction
{
    public class IMAPInstruction : IDisposable
    {
        private ProviderType providerType;
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private String response;

        private const String ReadOverPattern = @"\bA\d{2}\s+(?<result>[A-Z]{2})";

        public IMAPInstruction(ProviderType providerType, StreamWriter streamWriter, StreamReader streamReader)
        {
            this.providerType = providerType;
            this.streamWriter = streamWriter;
            this.streamReader = streamReader;
        }

        /// <summary>
        ///  CAPABILITY指令返回服务器所支持的功能
        /// </summary>
        /// <returns></returns>
        public String CAPABILITYRepsonse()
        {
            streamWriter.WriteLine("A21 CAPABILITY ");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  NOOP指令返回一个肯定响应,防止长时间处于不活动状态导致连接中断
        /// </summary>
        /// <returns></returns>
        public String NOOPResponse()
        {
            streamWriter.WriteLine("A22 NOOP ");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  LOGIN指令输入账户密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean LOGINResponse(String account, String password)
        {
            streamWriter.WriteLine("A01 LOGIN " + account + " " + password);
            streamWriter.Flush();
            Boolean result;
            response = readToEnd(out result);
            return result;
        }

        /// <summary>
        ///  LIST指令返回所有信箱名称
        /// </summary>
        /// <returns></returns>
        public String LISTResponse()
        {
            streamWriter.WriteLine("A02 LIST \"\" *");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  SELECT指令选择邮箱内文件夹(收件箱,草稿箱...)
        /// </summary>
        /// <param name="mailboxName">收件箱为'INBOX'</param>
        /// <returns></returns>
        public String SELECTResponse(String mailboxName)
        {
            streamWriter.WriteLine("A03 SELECT \"" + mailboxName + "\"");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  SELECT指令查找邮件
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public String SEARCHResponse(String expression)
        {
            streamWriter.WriteLine("A04 SEARCH " + expression);
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  FETCH指令返回邮件的相关信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public String FETCHResponse(String expression)
        {
            streamWriter.WriteLine("A05 FETCH " + expression);
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  UID FETCH指令根据UID获取邮件
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public String UIDFETCHResponse(String expression)
        {
            streamWriter.WriteLine("A10 UID FETCH " + expression);
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  STORE指令保存邮件状态
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public String STOREResponse(String expression)
        {
            streamWriter.WriteLine("A06 STORE " + expression);
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  COPY指令将邮件复制到指定邮箱
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <param name="mailBoxName"></param>
        /// <returns></returns>
        public String COPYResponse(Int32 mailIndex, String mailBoxName)
        {
            streamWriter.WriteLine("A07 COPY " + mailIndex + " \"" + mailBoxName + "\"");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  CLOSE退出所进邮箱
        /// </summary>
        /// <returns></returns>
        public String CLOSEResponse()
        {
            streamWriter.WriteLine("A08 CLOSE ");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  LOGOUT指令退出登录
        /// </summary>
        /// <returns></returns>
        public String LOGOUTResponse()
        {
            streamWriter.WriteLine("A09 LOUGOUT ");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        #region SEARCH 常用操作封装

        /// <summary>
        ///  SELECT指令查找邮件
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public String SEARCHResponse(SearchType searchType)
        {
            streamWriter.WriteLine("A04 SEARCH " + searchType.ToString());
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        #endregion

        #region STORE 常用操作封装
        /// <summary>
        ///  STORE指令保存邮件标志 添加标志
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <param name="storeFlags"></param>
        /// <returns></returns>
        public String STOREAddFlagResponse(Int32 mailIndex, StoreFlags storeFlags)
        {
            streamWriter.WriteLine("A06 STORE " + mailIndex + " +FLAGS (\\" + storeFlags.ToString() + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  STORE指令保存邮件标志 添加标志
        /// </summary>
        /// <param name="startMailIndex"></param>
        /// <param name="endMailIndex"></param>
        /// <param name="storeFlags"></param>
        /// <returns></returns>
        public String STOREAddFlagResponse(Int32 startMailIndex, Int32 endMailIndex, StoreFlags storeFlags)
        {
            streamWriter.WriteLine("A06 STORE " + startMailIndex + ":" + endMailIndex + " +FLAGS (\\" + storeFlags.ToString() + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  STORE指令保存邮件标志 减去标志
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <param name="storeFlags"></param>
        /// <returns></returns>
        public String STOREReduceFlagResponse(Int32 mailIndex, StoreFlags storeFlags)
        {
            streamWriter.WriteLine("A06 STORE " + mailIndex + " -FLAGS (\\" + storeFlags.ToString() + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  STORE指令保存邮件标志 减去标志
        /// </summary>
        /// <param name="startMailIndex"></param>
        /// <param name="endMailIndex"></param>
        /// <param name="storeFlags"></param>
        /// <returns></returns>
        public String STOREReduceFlagResponse(Int32 startMailIndex, Int32 endMailIndex, StoreFlags storeFlags)
        {
            streamWriter.WriteLine("A06 STORE " + startMailIndex + ":" + endMailIndex + " -FLAGS (\\" + storeFlags.ToString() + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        #endregion

        #region UIDSTORE 常用封装

        /// <summary>
        ///  根据UID添加标志
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="storeFlags"></param>
        /// <returns></returns>
        public String UIDSTOREADDFlagResponse(String UID, StoreFlags storeFlags)
        {
            streamWriter.WriteLine("A11 UID STORE " + UID + " +FLAGS (\\" + storeFlags.ToString() + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        /// <summary>
        ///  根据UID删去标志
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="storeFlags"></param>
        /// <returns></returns>
        public String UIDSTOREReduceFlagRespponse(String UID, StoreFlags storeFlags)
        {
            streamWriter.WriteLine("A11 UID STORE " + UID + " -FLAGS (\\" + storeFlags.ToString() + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        public String UIDSTORFlagsReponse(String UID, String flags)
        {
            streamWriter.WriteLine("A11 UID STORE " + UID + " FLAGS (" + flags + ")");
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        #endregion

        #region FETCH 常用操作封装

        /// <summary>
        ///  FETCH指令返回邮件的相关信息
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <param name="fetchType">处理的参数</param>
        /// <returns></returns>
        public String FETCHResponse(Int32 mailIndex, FetchType fetchType)
        {
            streamWriter.WriteLine("A05 FETCH " + mailIndex + " " + fetchType.ToString());
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        #endregion

        #region UIDFETCH 常用封装

        public String UIDFETCHResponse(String UID, FetchType fetchType)
        {
            streamWriter.WriteLine("A10 UID FETCH " + UID + " " + fetchType.ToString());
            streamWriter.Flush();
            response = readToEnd();
            return response;
        }

        #endregion

        private String readToEnd()
        {
            Boolean result = true;
            return readToEnd(out result);
        }

        private String readToEnd(out Boolean result)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            Match match;
            try
            {
                do
                {
                    response = streamReader.ReadLine();
                    match = Regex.Match(response, ReadOverPattern);
                    if (!match.Success)
                        stringBuilder.AppendLine(response);
                } while (!match.Success);
                result = match.Groups["result"].ToString() == "OK";
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #region Dispose

        private Boolean disposed = false;
        private void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {


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

        #endregion

        #region 析构函数

        ~IMAPInstruction()
        {
            Dispose(false);
        }

        #endregion
    }
}
