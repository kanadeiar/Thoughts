using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace Thoughts.WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users/[action]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly ILogger<UsersApiController> _logger;

        public UsersApiController(ILogger<UsersApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersSkip(CancellationToken ct, int skip = 0, int take = 5)
        {

            return Ok();
        }
    }
}
