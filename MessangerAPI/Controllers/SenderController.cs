using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessangerContacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MessangerAPI.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class SenderController : ControllerBase
    {
        IMessanger _messanger;
        ILogger<SenderController> _logger;
        public SenderController(IMessanger messanger, ILogger<SenderController> logger)
        {
            _messanger = messanger;
            _logger = logger;
        }
       
        
        // POST api/<SenderController>
        [HttpPost]
        public async Task Send(MessageRequest messageRequest)
        {
            await _messanger.Send(messageRequest.ID, messageRequest.MessageBody);
        }

        
    }
}
