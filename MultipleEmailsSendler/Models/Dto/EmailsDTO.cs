using System.Collections.Generic;

namespace MultipleEmailsSendler.Models.Dto
{
    public class EmailsDTO
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string MailFrom { get; set; }

        public IEnumerable<RecipientsDTO> Recipients { get; set; }

    }
}
