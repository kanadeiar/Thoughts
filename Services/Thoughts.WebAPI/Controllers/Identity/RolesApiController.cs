using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Idetity;
using Thoughts.Identity.DAL;

using WebStore.Interfaces.Services;

namespace Thoughts.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(WebAPIAddresses.Addresses.Identity.Roles)]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleManager<IdentRole> _RoleManager;
        private readonly ILogger<RolesApiController> _Logger;

        public RolesApiController(IdentityDB db, RoleManager<IdentRole> roleManager, ILogger<RolesApiController> Logger)
        {
            _Logger = Logger;
            _RoleManager = roleManager;
        }

        /// <summary>
        /// Получить список ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<IdentRole>> GetAll() => await _RoleManager.Roles.ToArrayAsync();

        /// <summary>
        /// Создать новую роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CreateAsync(IdentRole role)
        {
            var creation_result = await _RoleManager.CreateAsync(role);

            if (!creation_result.Succeeded)
                _Logger.LogWarning("Ошибка создания роли {0}:{1}",
                    role,
                    string.Join(", ", creation_result.Errors.Select(e => e.Description)));

            return creation_result.Succeeded;
        }

        /// <summary>
        /// Создать новую роль
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("{name}")]
        public async Task<bool> CreateAsync(string name)
        {
            var role = new IdentRole() { Name = name, NormalizedName = name.ToUpper() };
            var creation_result = await _RoleManager.CreateAsync(role);

            if (!creation_result.Succeeded)
                _Logger.LogWarning("Ошибка создания роли {0}:{1}",
                    role,
                    string.Join(", ", creation_result.Errors.Select(e => e.Description)));

            return creation_result.Succeeded;
        }

        /// <summary>
        /// Изменить роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdateAsync(IdentRole role)
        {
            var uprate_result = await _RoleManager.UpdateAsync(role);

            if (!uprate_result.Succeeded)
                _Logger.LogWarning("Ошибка изменения роли {0}:{1}",
                    role,
                    string.Join(", ", uprate_result.Errors.Select(e => e.Description)));

            return uprate_result.Succeeded;
        }

        /// <summary>
        /// Удалить роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(IdentRole role)
        {
            var delete_result = await _RoleManager.DeleteAsync(role);

            if (!delete_result.Succeeded)
                _Logger.LogWarning("Ошибка удаления роли {0}:{1}",
                    role,
                    string.Join(", ", delete_result.Errors.Select(e => e.Description)));

            return delete_result.Succeeded;
        }

        /// <summary>
        /// Найти роль по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("FindById/{id}")]
        public async Task<IdentRole> FindByIdAsync(string id) => await _RoleManager.FindByIdAsync(id);

        /// <summary>
        /// Найти роль по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("FindByName/{name}")]
        public async Task<IdentRole> FindByNameAsync(string name) => await _RoleManager.FindByNameAsync(name);

        /// <summary>
        /// Изменить название роли 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("SetRoleName/{name}")]
        public async Task<string> SetRoleNameAsync(IdentRole role, string name)
        {
            await _RoleManager.SetRoleNameAsync(role, name);
            await _RoleManager.UpdateAsync(role);
            return role.Name;
        }

        /// <summary>
        /// Получить id роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] IdentRole role) => await _RoleManager.GetRoleIdAsync(role);

        /// <summary>
        /// Получить название роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] IdentRole role) => await _RoleManager.GetRoleNameAsync(role);
    }
}
