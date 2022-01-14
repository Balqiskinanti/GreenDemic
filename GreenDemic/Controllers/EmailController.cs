using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenDemic.Models;
using System.Net.Mail;

namespace GreenDemic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        public ActionResult SendMail(Email e)
        {
            string subject = e.subject;
            string body = e.body;
            string to = e.to;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("contact.greendemic@gmail.com");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = true;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("contact.greendemic@gmail.com", "GreenDemic_2021");
            smtp.Send(mail);

            return Ok();
        }
    }
}
