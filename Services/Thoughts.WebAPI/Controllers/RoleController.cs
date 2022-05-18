using Microsoft.AspNetCore.Mvc;

using Thoughts.DAL.Entities.Base;
using Thoughts.Interfaces.Base.Repositories;

namespace Thoughts.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRepository<Role> _repository;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRepository<Role> repository, ILogger<RoleController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancel = default) => Ok(await _repository.GetAllAsync(cancel));

    [HttpPost("Add")]
    public async Task<IActionResult> AddRole(string roleName, CancellationToken cancel = default)
    {
        var role = new Role { Name = roleName };
        await _repository.AddAsync(role, cancel);
        return Ok(role);
    }
}