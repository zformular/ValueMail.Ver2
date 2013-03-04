using System;

namespace ValueMail.Receive.IMAP.Infrastructure
{
    public class FetchType
    {
        private String name;

        public FetchType(String value)
        {
            name = value;
        }

        public override string ToString()
        {
            return name;
        }

        public static explicit operator FetchType(String value)
        {
            return new FetchType(value);
        }
        /// <summary>
        ///  返回文件内容的编码及boundary
        /// </summary>
        public static FetchType BODYSTRUCTURE = (FetchType)"BODYSTRUCTURE";
        /// <summary>
        ///  返回邮件的唯一码
        /// </summary>
        public static FetchType UID = (FetchType)"UID";
        /// <summary>
        ///  返回邮件头
        /// </summary>
        public static FetchType RFC822_HEADER = (FetchType)"RFC822.HEADER";
        /// <summary>
        ///  返回邮件体
        /// </summary>
        public static FetchType RFC822_TEXT = (FetchType)"RFC822.TEXT";
        /// <summary>
        ///  返回整个邮件
        /// </summary>
        public static FetchType RFC822 = (FetchType)"RFC822";
        /// <summary>
        ///  查看邮件的标志
        /// </summary>
        public static FetchType FLAGS = (FetchType)"FLAGS";
        /// <summary>
        ///  自定义Fetch的参数
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static FetchType Expression(String expression)
        {
            return (FetchType)expression;
        }
    }
}
