using System;

namespace WebApi.Dtos
{
    public class MessagesDto
    {
        public string PostedBy { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}