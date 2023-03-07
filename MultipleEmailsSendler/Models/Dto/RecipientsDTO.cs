
using System;

namespace MultipleEmailsSendler.Models.Dto
{
    public class RecipientsDTO
    {
        public string Recipient { get; set; }

        public DateTime? SendDate { get; set; }

        public string SendState { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
