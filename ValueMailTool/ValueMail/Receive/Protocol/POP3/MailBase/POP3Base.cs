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
using System.Collections.Generic;
using ValueMail.Receive.Model;
using ValueMail.Receive.Infrastructure;
using ValueMail.Receive.POP3.Instruction;

namespace ValueMail.Receive.POP3.MailBase
{
    public class POP3Base : ValueMail.Receive.MailBase.MailBase
    {
        private POP3Instruction Instruction = null;

        private const String pointBoundary = ".";
        private const String emptyBoundary = "";

        /// <summary>
        ///  连接服务器
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public override void Connect(string server, int port)
        {
            Connect(server, port, false);
        }

        public override void Connect(string server, int port, bool ssl)
        {
            base.Connect(server, port, ssl);
            // 实例化指令集合类(因为在建立连接后参数才有实例... - -!)
            Instruction = new POP3Instruction(providerType, streamWriter, streamReader);
        }

        /// <summary>
        ///  登陆账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public override Boolean Loging(string account, string password)
        {
            if (String.IsNullOrEmpty(account))
                throw new ArgumentException("账号不能为空");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空");

            try
            {
                response = Instruction.USERResponse(account);
                response = Instruction.PASSResponse(password);
                if (response[0] == '-')
                    return false;
                else
                    return true;
            }
            catch (ArgumentException aex)
            {
                throw (aex);
            }
            catch (Exception)
            {
                throw new Exception("与服务器连接失败");
            }
        }

        /// <summary>
        ///  获得邮件数量
        /// </summary>
        /// <returns></returns>
        public Int32 GetMailCount()
        {
            response = Instruction.STATResponse();

            // 数据用字符串分割
            String[] count = response.Split(' ');
            // 下标为1的保存了邮件个数
            var totalMessage = Convert.ToInt32(count[1]);
            if (totalMessage > 0)
                return totalMessage;
            else
                return 0;
        }

        /// <summary>
        ///  获得邮件头部内容
        /// </summary>
        public override List<MailHeadModel> GetMailHeaders()
        {
            List<MailHeadModel> headerList = new List<MailHeadModel>();
            Int32 mailCount = this.GetMailCount();
            for (int index = 1; index <= mailCount; index++)
            {
                headerList.Add(this.getMailHeader(index));
            }
            return headerList;
        }

        /// <summary>
        ///  根据编号获得邮件头部内容
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private MailHeadModel getMailHeader(Int32 index)
        {
            String uid = Instruction.UIDLResponse(index).Split(' ')[2];
            response = Instruction.TOPResponse(index);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            while (true)
            {
                response = Instruction.ReadResponse();
                if (response == pointBoundary)
                    break;
                stringBuilder.AppendLine(response);
            }
            return ReceiveHelper.GetMailHead(uid, stringBuilder.ToString());
        }

        public override MailModel GetMail(Int32 index)
        {
            String uid = Instruction.UIDLResponse(index).Split(' ')[2];
            response = Instruction.RETRResponse(index);

            System.Text.StringBuilder headSb = new System.Text.StringBuilder();
            while (true)
            {
                response = Instruction.ReadResponse();
                if (response == emptyBoundary)
                    break;
                else
                {
                    headSb.AppendLine(response);
                }
            }
            System.Text.StringBuilder bodySb = new System.Text.StringBuilder();
            while (true)
            {
                response = Instruction.ReadResponse();
                if (response == pointBoundary)
                    break;
                else
                {
                    bodySb.AppendLine(response);
                }
            }
            return ReceiveHelper.GetMail(uid, headSb.ToString(), bodySb.ToString());
        }

        public override void DeleMail(int index)
        {
            response = Instruction.DELEResponse(index);
        }

        public override List<MailModel> GetMails()
        {
            List<MailModel> mailList = new List<MailModel>();
            Int32 mailCount = this.GetMailCount();
            for (int index = 1; index <= mailCount; index++)
            {
                mailList.Add(GetMail(index));
            }
            return mailList;
        }

        public override void ResetStatus()
        {
            Instruction.RSETResponse();
        }

        public override void Disconnect()
        {
            Instruction.QUITResponse();
            base.Disconnect();
        }

        /// <summary>
        ///  释放资源
        /// </summary>  
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Instruction != null)
            {
                Instruction.Dispose();
                Instruction = null;
            }
        }

        #region IDisposable 成员

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 析构函数

        ~POP3Base()
        {
            Dispose(false);
        }

        #endregion
    }
}
