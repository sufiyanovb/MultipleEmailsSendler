using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MailsController(AppDataContext context, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _emailsCommandRepository = new EfGenericCommandRepository<Emails>(_context);
            _mapper = mapper;
        }

        /// <summary>  
        ///  Получение всех емейлов и связанных с ними получателей
        /// </summary> 
        [HttpGet]
        public async Task<IEnumerable<EmailsDTO>> Mails()
        {
            var emails = await Task.Run(() => _context.Emails.Include(i => i.Recipients));
            return _mapper.Map<List<EmailsDTO>>(emails);
        }


        // POST api/values
        /// <summary>  
        ///  Сохранение записей в БД,после чего происходит отправка сообщения получателям
        /// </summary> 
        [HttpPost]

        public IActionResult Post([FromBody] Emails input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.MailFrom = _configuration["userNameMail"];

            _emailsCommandRepository.Create(input);

            new EmailSendler(_configuration, _context, input).SendEmail();

            EmailsDTO ret = new EmailsDTO();

            _mapper.Map(input, ret);

            return Ok(ret);

        }
    }
}
