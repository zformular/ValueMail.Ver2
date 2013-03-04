/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 10.1.2012              |
 |                                            |
 ╰==========================================╯

*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ValueMail.Receive.Model;
using ValueMail.Receive.Instruction;
using ValueMail.Receive.Infrastructure;
using ValueMail.Receive.IMAP.Infrastructure;

namespace ValueMail.Receive.IMAP.MailBase
{
    public class IMAPBase : ValueMail.Receive.MailBase.MailBase
    {
        private IMAPInstruction Instruction = null;

        public override void Connect(string server, int port)
        {
            Connect(server, port, false);
        }

        public override void Connect(string server, int port, bool ssl)
        {
            base.Connect(server, port, ssl);
            Instruction = new IMAPInstruction(providerType, streamWriter, streamReader);
        }

        public override Boolean Loging(string account, string password)
        {
            if (String.IsNullOrEmpty(account))
                throw new ArgumentException("账号不能为空");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空");

            try
            {
                Boolean result = Instruction.LOGINResponse(account, password);
                return result;
            }
            catch (ArgumentException aex)
            {
                throw (aex);
            }
            catch (Exception ex)
            {
                throw new Exception("与服务器连接失败");
            }
        }

        /// <summary>
        ///  返回邮箱列表
        /// </summary>
        /// <returns></returns>
        public String ListMailbox()
        {
            return Instruction.LISTResponse();
        }

        /// <summary>
        ///  进入指定的邮箱
        /// </summary>
        /// <param name="mailboxName"></param>
        public void SelectMailbox(String mailboxName)
        {
            response = Instruction.SELECTResponse(mailboxName);
        }

        public override List<MailHeadModel> GetMailHeaders()
        {
            return GetMailHeaders(SearchType.New);
        }

        public override List<MailHeadModel> GetMailHeaders(SearchType searchType)
        {
            return GetMailHeaders(searchType.ToString());
        }

        public override List<MailHeadModel> GetMailHeaders(String expression)
        {
            List<MailHeadModel> mailheadList = new List<MailHeadModel>();
            response = Instruction.SEARCHResponse(expression);
            MatchCollection maches = Regex.Matches(response, IMAPHelper.SearchMailPattern);
            foreach (Match item in maches)
            {
                Int32 index = Int32.Parse(item.Groups["index"].Value);
                mailheadList.Add(getMailHeader(index));
            }
            return mailheadList;
        }

        private MailHeadModel getMailHeader(Int32 index)
        {
            // 读取邮件的UID 和 标记
            response = Instruction.FETCHResponse(index, FetchType.Expression("(UID FLAGS)"));
            String uid = Regex.Match(response, IMAPHelper.UIDPattern).Groups["uid"].Value;
            String flags = Regex.Match(response, IMAPHelper.Flags).Groups["flags"].Value;

            // 获取邮件头信息
            response = Instruction.FETCHResponse(index, FetchType.Expression("BODY[HEADER.FIELDS (Date From Subject Content-Type Content-Transfer-Encoding)]"));
            String body = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].Value;

            // 还原邮件标记
            response = Instruction.UIDSTORFlagsReponse(uid, flags);

            return ReceiveHelper.GetMailHead(uid, body);
        }

        public override MailModel GetMail(int index)
        {
            // 读取邮件的UID 和 标记
            response = Instruction.FETCHResponse(index, FetchType.Expression("(UID FLAGS)"));
            String uid = Regex.Match(response, IMAPHelper.UIDPattern).Groups["uid"].Value;
            String flags = Regex.Match(response, IMAPHelper.Flags).Groups["flags"].Value;

            // 获取邮件信息
            response = Instruction.FETCHResponse(index, FetchType.RFC822_HEADER);
            String head = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].Value;
            response = Instruction.FETCHResponse(index, FetchType.RFC822_TEXT);
            String body = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].Value;

            // 还原邮件标记
            response = Instruction.UIDSTORFlagsReponse(uid, flags);

            return ReceiveHelper.GetMail(uid, head, body);
        }

        public MailModel GetMailByUID(String UID)
        {
            // 读取邮件的标记
            response = Instruction.UIDFETCHResponse(UID, FetchType.FLAGS);
            String flags = Regex.Match(response, IMAPHelper.Flags).Groups["flags"].Value;

            // 获取邮件信息
            response = Instruction.UIDFETCHResponse(UID, FetchType.RFC822_HEADER);
            String head = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].Value;
            response = Instruction.UIDFETCHResponse(UID, FetchType.RFC822_TEXT);
            String body = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].Value;
            response = Instruction.UIDFETCHResponse(UID, FetchType.Expression("BODY()"));

            // 还原邮件标记
            response = Instruction.UIDSTORFlagsReponse(UID, flags);

            return ReceiveHelper.GetMail(UID, head, body);
        }

        /// <summary>
        ///  根据UID添加标志
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="flag"></param>
        public void StoreAddFlag(String UID, StoreFlags flag)
        {
            response = Instruction.UIDSTOREADDFlagResponse(UID, flag);
        }

        /// <summary>
        ///  根据UID删去标志
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="flag"></param>
        public void StoreReduceFlag(String UID, StoreFlags flag)
        {
            response = Instruction.UIDSTOREReduceFlagRespponse(UID, flag);
        }

        public void CopyMail(Int32 index, String mailbox)
        {
            Instruction.COPYResponse(index, mailbox);
        }

        public override void DeleMail(int index)
        {
            Instruction.STOREAddFlagResponse(index, StoreFlags.Deleted);
        }

        public void CloseMailbox()
        {
            Instruction.CLOSEResponse();
        }

        public override void Disconnect()
        {
            Instruction.LOGOUTResponse();
            base.Disconnect();
        }

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Instruction != null)
            {
                Instruction.Dispose();
                Instruction = null;
            }
        }
    }
}
