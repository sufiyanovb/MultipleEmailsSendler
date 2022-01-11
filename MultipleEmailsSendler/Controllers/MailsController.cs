using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MultipleEmailsSendler.Models;
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
        private readonly EfGenericQueryRepository<Recipients> _recipientscQueryRepository;


        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;

        public MailsController(AppDataContext context, IConfiguration configuration)
        {
            _configuration = configuration;

            _context = context;

            _emailsCommandRepository = new EfGenericCommandRepository<Emails>(_context);
            _recipientsCommandRepository = new EfGenericCommandRepository<Recipients>(_context);

            _emailsQueryRepository=new EfGenericQueryRepository<Emails>(context);
            _recipientscQueryRepository=new EfGenericQueryRepository<Recipients>(context);
        }

        /// <summary>  
        ///  Получение всех емейлов и связанных с ними получателей
        /// </summary> 
        [HttpGet]
        public async Task<IEnumerable<Emails>> Mails()
        {
            return await Task.Run(() => _emailsCommandRepository.GetWithInclude(i => i.Recipients));
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

            _emailsQueryRepository.Create(email);

            foreach (var rec in data.Recipients)
            {
                _recipientscQueryRepository.Create(new Recipients { EmailId = email.Id, Recipient = rec.Recipient });
            }

            new EmailSendler(_configuration, _context, email).SendEmail();

            return Ok(email);

        }
    }
}
