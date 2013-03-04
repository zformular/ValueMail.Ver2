using System;
using System.Net.Mime;

namespace ValueMail.Receive.Model
{
    public class PartialMailModel
    {
        public ContentType ContentType { get; set; }

        public String ContentTransferEncoding { get; set; }

        public ContentDisposition ContentDisposition { get; set; }

        public String Context { get; set; }

        public Boolean HasValue { get; private set; }

        public PartialMailModel(Boolean hasValue)
        {
            this.HasValue = hasValue;
        }
    }
}
