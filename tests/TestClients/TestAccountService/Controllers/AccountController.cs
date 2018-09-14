using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using MessageLibrary.Contracts;
using Microsoft.AspNetCore.Mvc;
using TestAccountService.Models;

namespace TestAccountService.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
	    private readonly IBus _bus;
	    private static int _count;

	    public AccountController(IBus bus)
	    {
		    _bus = bus;
	    }

	    // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUser user)
        {
	        var userEndPoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/User"));

	        user.LastName = $"{user.LastName} {_count++}";

	        await userEndPoint.Send<IAddUser>(user);

	        return Created("", user);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
