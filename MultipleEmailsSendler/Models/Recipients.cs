
using System;
using System.Text.Json.Serialization;

namespace MultipleEmailsSendler.Models
{
    /// <summary>  
    ///  Класс,описывающий сущность Получателей письма
    /// </summary> 
    public class Recipients
    {
        public Recipients()
        {

        }

        public int Id { get; set; }

        /// <summary>  
        ///  Ключевое поле для связи с основной записью(Email)
        /// </summary> 

        public int EmailId { get; set; }

        [JsonIgnore]
        public virtual Emails Email { get; set; }


        /// <summary>  
        ///  Дата отправки
        /// </summary> 
        public DateTime? SendDate { get; set; }

        /// <summary>  
        ///  статус доставки
        /// </summary> 
        public string SendState { get; set; }

        /// <summary>  
        ///  Текст ошибки
        /// </summary>  
        public string ExceptionMessage { get; set; }

        /// <summary>  
        ///  Адрес получателя
        /// </summary> 
        public string Recipient { get; set; }
    }
}
