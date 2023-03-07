using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MultipleEmailsSendler.Models;
using MultipleEmailsSendler.Models.Dto;
using MultipleEmailsSendler.Service;

namespace MultipleEmailsSendler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly EfGenericCommandRepository<Emails> _emailsCommandRepository;
        private readonly EfGenericQueryRepository<Emails> _emailsQueryRepository;
        private readonly EfGenericCommandRepository<Recipients> _recipientsCommandRepository;

        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;

        public MailsController(AppDataContext context, IConfiguration configuration)
        {
            _configuration = configuration;

            _context = context;

            _emailsCommandRepository = new EfGenericCommandRepository<Emails>(_context);
            _recipientsCommandRepository = new EfGenericCommandRepository<Recipients>(_context);

            _emailsQueryRepository = new EfGenericQueryRepository<Emails>(_context);

        }

        /// <summary>  
        ///  Получение всех емейлов и связанных с ними получателей
        /// </summary> 
        [HttpGet]
        public async Task<IEnumerable<EmailsDTO>> Mails()
        {
            return await Task.Run(() => _context.Emails.Select(i => new EmailsDTO()
            {
                Id = i.Id,
                Body = i.Body,
                MailFrom = i.MailFrom,
                Subject = i.Subject,
                Recipients = i.Recipients.Select(j => new RecipientsDTO()
                {
                    ExceptionMessage = j.ExceptionMessage,
                    Recipient = j.Recipient,
                    SendDate = j.SendDate,
                    SendState = j.SendState
                })
            }));
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

            _emailsCommandRepository.Create(email);

            foreach (var rec in data.Recipients)
            {
                _recipientsCommandRepository.Create(new Recipients { EmailId = email.Id, Recipient = rec.Recipient });
            }

            new EmailSendler(_configuration, _context, email).SendEmail();

            var ret = new EmailsDTO()
            {
                Id= email.Id,
                Subject = email.Subject,
                Body = email.Body,
                MailFrom = email.MailFrom,
                Recipients = email.Recipients.Select(j => new RecipientsDTO()
                {
                    ExceptionMessage = j.ExceptionMessage,
                    Recipient = j.Recipient,
                    SendDate = j.SendDate,
                    SendState = j.SendState
                })
            };

            return Ok(ret);

        }
    }
}
