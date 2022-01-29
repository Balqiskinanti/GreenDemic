﻿using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
