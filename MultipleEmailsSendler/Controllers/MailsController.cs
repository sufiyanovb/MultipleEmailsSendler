using System.Collections.Generic;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using MultipleEmailsSendler.Models;
using MultipleEmailsSendler.Service;

namespace MultipleEmailsSendler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly EfGenericRepository<Emails> _emailsRepository;
        private readonly EfGenericRepository<Recipients> _recipientsRepository;
        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;

        public MailsController(AppDataContext context, IConfiguration configuration)
        {
            _context = context;

            _emailsRepository = new EfGenericRepository<Emails>(_context);
            _recipientsRepository = new EfGenericRepository<Recipients>(_context);

            _configuration = configuration;
        }

        /// <summary>  
        ///  Получение всех емейлов и связанных с ними получателей
        /// </summary> 
        [HttpGet]
        public IEnumerable<Emails> Mails()
        {
            return _context.Emails.Include(i => i.Recipients);
        }
        // POST api/values
        /// <summary>  
        ///  Сохранение записей в БД,после чего происходит отправка сообщения получателям
        /// </summary> 
        [HttpPost]
        public IActionResult Post([FromBody] Emails data)
        {
            if (data == null)
            {
                return BadRequest();
            }

            var email = new Emails()
            {
                Subject = data.Subject,
                Body = data.Body,
                MailFrom = _configuration["userNameMail"]
            };

            _emailsRepository.Create(email);

            foreach (var rec in data.Recipients)
            {
                _recipientsRepository.Create(new Recipients { EmailId = email.Id, Recipient = rec.Recipient });

            }

            new EmailSendler(_configuration, _context, email).SendEmail();

            return Ok(email);

        }
    }
}
