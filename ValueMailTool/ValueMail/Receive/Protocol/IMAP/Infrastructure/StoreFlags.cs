using System;

namespace ValueMail.Receive.IMAP.Infrastructure
{
    /// <summary>
    ///  IMAP邮件Store的标志注意调用要加\在前方
    /// </summary>
    public enum StoreFlags
    {
        /// <summary>
        ///  删除标记
        /// </summary>
        Deleted,
        /// <summary>
        ///  已读标记
        /// </summary>
        Seen,
        /// <summary>
        ///  草稿标记
        /// </summary>
        Draft,
        /// <summary>
        ///  回复标记
        /// </summary>
        Answered
    }
}
