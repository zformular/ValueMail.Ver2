using System;

namespace ValueMail.Receive.IMAP.Infrastructure
{
    public class IMAPHelper
    {
        /// <summary>
        ///  匹配邮箱列表
        ///  原型:
        ///  * LIST (\HasNoChildren) "/" "INBOX"
        ///  * LIST (\HasNoChildren) "/" "Sent Messages"
        ///  
        ///  参数mailbox获取邮箱名称
        /// </summary>
        public const String MailBoxPattern = @"\*\s+LIST\s+\((\\.*)*\)\s+\S+\s+""(?<mailbox>.+)""";

        /// <summary>
        ///  搜索邮件的索引
        ///  原型"* SEARCH 1 2 3 4 8 9 10 11 15\r\n 13 15 16 ...";
        /// </summary>
        public const String SearchMailPattern = @"\s(?<index>\d+)";

        /*
         * 1 FETCH (BODY[HEADER.FIELDS (DATE FROM SUBJECT)] {275}
            From: "=?gb18030?B?16Ky4bLiytQ=?=" <2609064295@qq.com>
            Subject: =?gb18030?B?1eLKx9K7t+LT0NPKvP6y4srU08O1xLLiytTTyrz+?=
             =?gb18030?B?08PAtMq1z9bX1Ly6tcTTyrz+veLO9sDgserM4tfj?=
             =?gb18030?B?ubuzpLHIvc+55re2LMv50tTO0r7N0LTV4sO0s6Q=?=
            Date: Tue, 2 Oct 2012 20:53:20 +0800
         */
        /// <summary>
        ///  Fetch邮件出来的内容
        /// </summary>
        public const String FetchMailPattern = @"\*\s+\d+\s+FETCH\s+\(.*(\r\n)?(?<body>(.*(\r\n)?)*)(\r\n)+\)";

        /// <summary>
        ///  邮件标记模板
        /// </summary>
        public const String Flags = @"FLAGS\s+\((?<flags>[\\|a-zA-z]*)\)";

        /*
         * 1 FETCH (UID 22)
         */

        public const String UIDPattern = @"UID\s+(?<uid>\d+)";

    }
}
