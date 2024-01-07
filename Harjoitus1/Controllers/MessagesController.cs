using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Harjoitus1.Models;
using Microsoft.CodeAnalysis.CSharp;
using Harjoitus1.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Harjoitus1.Middleware;

namespace Harjoitus1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly IMessageService _messageService;
        private readonly IUserAuthenticationService _authService;

        public MessagesController(IMessageService service, IUserAuthenticationService authService)
        {
            _authService = authService;
            _messageService = service;
        }

        // GET: api/Messages
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages()
        {
            return Ok(await _messageService.GetMessagesAsync());
        }

        // GET: api/search/searchtext
        [HttpGet("search/{searchtext}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> SearchMessages(string searchtext)
        {
            return Ok(await _messageService.SearchMessagesAsync(searchtext));
        }

        //GET: api/Message/sent/username
        [HttpGet("sent/{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMySentMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name)?.Value == username)
            {
                IEnumerable<MessageDTO>? list = await _messageService.GetSentMessagesAsync(username);
                if (list == null)
                {
                    return BadRequest();
                }
                return Ok(list);
            }
            return BadRequest();
        }


        //GET: api/Message/received/username
        [HttpGet("received/{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMyReceivedMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name)?.Value == username)
            {
                IEnumerable<MessageDTO>? list = await _messageService.GetReceivedMessagesAsync(username);
                if (list == null)
                {
                    return BadRequest();
                }
                return Ok(list);
            }
            return BadRequest();
        }




        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDTO>> GetMessage(long id)
        {
            MessageDTO message = await _messageService.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, MessageDTO message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            bool result = await _messageService.UpdateMessageAsync(message);
            if (!result)
            {
                return NotFound();
            }
            return BadRequest();


        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(MessageDTO message)
        {
            if (this.User.FindFirst(ClaimTypes.Name)?.Value == message.Sender)
            {
                MessageDTO newMessage = await _messageService.NewMessageAsync(message);

                if (newMessage == null)
                {
                    return Problem();
                }
                return CreatedAtAction("GetMessage", new { id = message.Id }, message);
            }
            return BadRequest();
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (await _authService.IsMyMessage(this.User.FindFirst(ClaimTypes.Name).Value, id))
            {
                if (await _messageService.DeleteMessageAsync(id))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
    
}
