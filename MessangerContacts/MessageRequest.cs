using System;

namespace MessangerContacts
{
    public class MessageBody
    {
        public string Code { get; set; }
    }
    public class MessageRequest
    {
        public string ID { get; set; }
        public MessageBody MessageBody { get; set; }
    }
}
