using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueMail.Model
{
    public class HostSet
    {
        /// <summary>
        ///  服务器名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///  关键字
        /// </summary>
        public String KeyWord { get; set; }

        /// <summary>
        ///  Smtp服务
        /// </summary>
        public SmtpModel SmtpHost { get; set; }

        /// <summary>
        ///  Pop服务
        /// </summary>
        public PopModel PopHost { get; set; }

        /// <summary>
        ///  Imap服务
        /// </summary>
        public ImapModel ImapHost { get; set; }
    }
}
