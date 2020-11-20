using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using MultipleEmailsSendler.Models;

namespace MultipleEmailsSendler.Service
{
    public class EmailSendler
    {
        private readonly IConfiguration _configuration;
        private readonly Emails _email;
        private readonly AppDataContext _context;
        private readonly EfGenericRepository<Recipients> _recipientsRepository;

        public EmailSendler(IConfiguration configuration, AppDataContext context, Emails email)
        {
            _email = email;
            _configuration = configuration;
            _context = context;
            _recipientsRepository = new EfGenericRepository<Recipients>(_context);
        }

        /// <summary>  
        ///  Метод отправки сообщений
        /// </summary> 
        public void SendEmail()
        {
            // отправитель - устанавливаем адрес и отображаемую в письме тему
            var from = new MailAddress(_configuration["userNameMail"], _email.Subject);

            foreach (var recipient in _email.Recipients)
            {
                try
                {
                    // кому отправляем
                    var to = new MailAddress(recipient.Recipient);
                    // создаем объект сообщения
                    var m = new MailMessage(from, to)
                    {
                        Subject = _email.Subject,
                        Body = $"<h2>{_email.Body}</h2>",
                        IsBodyHtml = true
                    };

                    using (var smtp = new SmtpClient(_configuration["Host"], int.Parse(_configuration["Port"])))
                    {
                        smtp.Credentials = new NetworkCredential(_configuration["userNameMail"], _configuration["Password"]);
                        smtp.EnableSsl = true;
                        smtp.Send(m);
                        recipient.SendState = SendStatesEnum.OK.ToString();
                    }
                }
                catch (Exception e)
                {
                    recipient.SendState = SendStatesEnum.Faillure.ToString();
                    recipient.ExceptionMessage = e.Message;

                }

                finally
                {
                    recipient.SendDate = DateTime.Now;
                    _recipientsRepository.Update(recipient);
                }
            }
        }
    }
}






