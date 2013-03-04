using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueMail.Model
{
    public abstract class HostModel
    {
        /// <summary>
        ///  地址
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        ///  端口
        /// </summary>
        public Int32 Port { get; set; }

        /// <summary>
        ///  Ssl加密
        /// </summary>
        public Boolean Ssl { get; set; }
    }

    public class SmtpModel : HostModel
    {

    }

    public class PopModel : HostModel
    {

    }

    public class ImapModel : HostModel
    {

    }
}
