using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ValueMail.Model;
using ValueHelper.FileHelper;

namespace ValueMail
{
    public class MailHelper
    {
        private static String fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailConfig.xml");

        private static HostSet[] hostSet = null;

        public static HostSet[] HostSet
        {
            get
            {
                if (hostSet == null)
                    hostSet = GetMailConfigs();
                return hostSet;
            }
        }

        public static void CreateConfigFile()
        {
            var fileHelper = FileManager.GetTextHelper();

            fileHelper.SetFileName(fileName);
            if (fileHelper.CreateFile())
            {
                fileHelper.WriteLine("<?xml version=\"1.0\" encoding=\"gb2312\" ?>", false);
                fileHelper.WriteLine("<MailConfigs></MailConfigs>", true);

                var xml = XElement.Load(fileName);
                var gmail = new XElement("Mail", new XAttribute("Name", "谷歌"), new XAttribute("KeyWord", "@Gmail."));
                gmail.Add(new XElement("SMTP", "smtp.gmail.com", new XAttribute("PORT", "587"), new XAttribute("SSL", "true")));
                gmail.Add(new XElement("POP", "pop.gmail.com", new XAttribute("PORT", "993"), new XAttribute("SSL", "true")));
                gmail.Add(new XElement("IMAP", "imap.gmail.com", new XAttribute("PORT", "993"), new XAttribute("SSL", "true")));
                xml.Add(gmail);

                var ne163 = new XElement("Mail", new XAttribute("Name", "网易163"), new XAttribute("KeyWord", "@163."));
                ne163.Add(new XElement("SMTP", "smtp.163.com", new XAttribute("PORT", "25"), new XAttribute("SSL", "false")));
                ne163.Add(new XElement("POP", "pop.163.com", new XAttribute("PORT", "110"), new XAttribute("SSL", "false")));
                ne163.Add(new XElement("IMAP", "imap.163.com", new XAttribute("PORT", "143"), new XAttribute("SSL", "false")));
                xml.Add(ne163);

                var ne126 = new XElement("Mail", new XAttribute("Name", "网易126"), new XAttribute("KeyWord", "@126."));
                ne126.Add(new XElement("SMTP", "smtp.126.com", new XAttribute("PORT", "25"), new XAttribute("SSL", "false")));
                ne126.Add(new XElement("POP", "pop.126.com", new XAttribute("PORT", "110"), new XAttribute("SSL", "false")));
                ne126.Add(new XElement("IMAP", "imap.126.com", new XAttribute("PORT", "143"), new XAttribute("SSL", "false")));
                xml.Add(ne126);

                var sina = new XElement("Mail", new XAttribute("Name", "新浪"), new XAttribute("KeyWord", "@sina."));
                sina.Add(new XElement("SMTP", "smtp.sina.com", new XAttribute("PORT", "25"), new XAttribute("SSL", "false")));
                sina.Add(new XElement("POP", "pop.sina.com", new XAttribute("PORT", "110"), new XAttribute("SSL", "false")));
                sina.Add(new XElement("IMAP", "imap.sina.com", new XAttribute("PORT", "143"), new XAttribute("SSL", "false")));
                xml.Add(sina);

                var tencent = new XElement("Mail", new XAttribute("Name", "新浪"), new XAttribute("KeyWord", "@qq."));
                tencent.Add(new XElement("SMTP", "smtp.qq.com", new XAttribute("PORT", "25"), new XAttribute("SSL", "false")));
                tencent.Add(new XElement("POP", "pop.qq.com", new XAttribute("PORT", "110"), new XAttribute("SSL", "false")));
                tencent.Add(new XElement("IMAP", "imap.qq.com", new XAttribute("PORT", "143"), new XAttribute("SSL", "false")));
                xml.Add(tencent);

                xml.Save(fileName);
            }
        }

        private static HostSet[] GetMailConfigs()
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                    CreateConfigFile();

                var xml = XElement.Load(fileName);
                var mails = xml.Elements("Mail");
                var models = new List<HostSet>();
                foreach (var item in mails)
                {
                    var set = new HostSet();
                    set.Name = item.Attribute("Name").Value;
                    set.KeyWord = item.Attribute("KeyWord").Value;

                    var smtp = item.Element("SMTP");
                    if (smtp != null)
                        set.SmtpHost = new SmtpModel
                        {
                            Host = smtp.Value,
                            Port = Int32.Parse(smtp.Attribute("PORT").Value),
                            Ssl = Boolean.Parse(smtp.Attribute("SSL").Value)
                        };

                    var pop = item.Element("POP");
                    if (pop != null)
                        set.PopHost = new PopModel
                        {
                            Host = pop.Value,
                            Port = Int32.Parse(pop.Attribute("PORT").Value),
                            Ssl = Boolean.Parse(pop.Attribute("SSL").Value)
                        };

                    var imap = item.Element("IMAP");
                    if (imap != null)
                        set.ImapHost = new ImapModel
                        {
                            Host = imap.Value,
                            Port = Int32.Parse(imap.Attribute("PORT").Value),
                            Ssl = Boolean.Parse(imap.Attribute("SSL").Value)
                        };

                    models.Add(set);
                }

                return models.ToArray();
            }
            catch
            {
                throw new ArgumentException("请确保邮箱配置正确");
            }
        }

        /// <summary>
        ///  获得邮件服务器参数
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static HostSet GetHost(String address)
        {
            var keyWord = Regex.Match(address, @"(?<mark>@.*\.)").Groups["mark"].Value;
            if (keyWord == String.Empty)
                throw new ArgumentException("请输入正确的邮箱地址");

            for (int i = 0; i < HostSet.Length; i++)
            {
                if (HostSet[i].KeyWord.ToLower() == keyWord.ToLower())
                    return HostSet[i];
            }

            return new HostSet();
        }
    }
}
