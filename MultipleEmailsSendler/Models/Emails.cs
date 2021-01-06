using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MultipleEmailsSendler.Models
{
    /// <summary>  
    ///  Класс,описывающий сущность Emall
    /// </summary> 
    public class Emails : IValidatableObject
    {
        public Emails()
        {
            Recipients = new List<Recipients>();
        }
        public int Id { get; set; }

        /// <summary>  
        ///  Тема письма
        /// </summary> 
        [Required(ErrorMessage = "Укажите тему письма!")]
        public string Subject { get; set; }

        /// <summary>  
        ///  Текст письма
        /// </summary> 
        [Required(ErrorMessage = "Укажите текст письма!")]
        public string Body { get; set; }

        /// <summary>  
        ///  От кого
        /// </summary> 
        //[Required(ErrorMessage = "Укажите отправителя!")]
        //[EmailAddress(ErrorMessage = "Некорректный адрес!")]
        public string MailFrom { get; set; }

        /// <summary>  
        ///  Ссылка на получателей письма
        /// </summary>  
        public virtual ICollection<Recipients> Recipients { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Recipients.Any())
                yield return new ValidationResult("Укажите хотя бы одного получателя!!!", new List<string>() { "Recipients" });
        }
    }
}
