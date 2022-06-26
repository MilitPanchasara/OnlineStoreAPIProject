using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJWTManagerRepository jWTManagerRepository;
        public UserController(IJWTManagerRepository jWTManagerRepository_)
        {
            jWTManagerRepository = jWTManagerRepository_;
        }
        // GET: api/<UserController>
        [ApiVersion("1")]
        [HttpGet]
        public List<string> GetV1()
        {
            var users = new List<string>
            {
                "Satinder Singh",
                "Amit Sarna",
                "Davin Jon"
            };

            return users;
        }

        [ApiVersion("2")]
        [HttpGet]
        public List<string> GetV2()
        {
            var users = new List<string>
            {
                "Satinder",
                "Amit",
                "Davin"
            };

            return users;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(User usersdata)
        {
            var token = jWTManagerRepository.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
