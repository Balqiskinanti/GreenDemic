using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
