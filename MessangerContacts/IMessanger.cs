using MessangerContracts;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MessangerContacts
{
    public interface IMessanger
    {
         Task Send(string id, MessageBody message);
        Task<IReceiver> Add(string id,ISocket socket);
    }
}
