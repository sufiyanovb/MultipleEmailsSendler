using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MultipleEmailsSendler.Models;
using MultipleEmailsSendler.Service;
using MultipleEmailsSendler.Service.Interfaces;

namespace MultipleEmailsSendler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly IGenericRepository<Emails> _emailsRepository;
        private readonly IGenericRepository<Recipients> _recipientsRepository;
        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;

        public MailsController(AppDataContext context, IConfiguration configuration)
        {
            _configuration = configuration;

            _context = context;

            _emailsRepository = new EfGenericRepository<Emails>(_context);
            _recipientsRepository = new EfGenericRepository<Recipients>(_context);
        }

        /// <summary>  
        ///  Получение всех емейлов и связанных с ними получателей
        /// </summary> 
        [HttpGet]
        public async Task<IEnumerable<Emails>> Mails()
        {
            return await Task.Run(() => _emailsRepository.GetWithInclude(i => i.Recipients));
        }
        // POST api/values
        /// <summary>  
        ///  Сохранение записей в БД,после чего происходит отправка сообщения получателям
        /// </summary> 
        [HttpPost]
        public IActionResult Post([FromBody] Emails data)
        {
            if (data == null)
                return BadRequest();

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
