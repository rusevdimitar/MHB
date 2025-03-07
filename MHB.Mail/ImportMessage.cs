using System;
using System.Collections.Generic;

namespace MHB.Mail
{
    public class ImportMessage
    {
        public string ApiKey { get; set; }
        public string SenderEmailAddress { get; set; }

        public DateTime DateSent { get; set; }
        public IEnumerable<ImportMessageItem> MessageItems { get; set; }
    }
}