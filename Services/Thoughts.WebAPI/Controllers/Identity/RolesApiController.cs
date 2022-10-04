using Identity.DAL;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Idetity;

using WebStore.Interfaces.Services;

namespace Thoughts.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(WebAPIAddresses.Addresses.Identity.Roles)]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role> _RoleStore;
        private readonly ILogger<RolesApiController> _Logger;

        public RolesApiController(IdentityDB db, ILogger<RolesApiController> Logger)
        {
            _Logger = Logger;
            _RoleStore = new(db);
        }

        /// <summary>
        /// Получить список ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAll() => await _RoleStore.Roles.ToArrayAsync();

        /// <summary>
        /// Создать новую роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CreateAsync(Role role)
        {
            var creation_result = await _RoleStore.CreateAsync(role);

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
            var role = new Role() { Name = name, NormalizedName = name.ToUpper() };
            var creation_result = await _RoleStore.CreateAsync(role);

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
        public async Task<bool> UpdateAsync(Role role)
        {
            var uprate_result = await _RoleStore.UpdateAsync(role);

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
        public async Task<bool> DeleteAsync(Role role)
        {
            var delete_result = await _RoleStore.DeleteAsync(role);

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
        public async Task<Role> FindByIdAsync(string id) => await _RoleStore.FindByIdAsync(id);

        /// <summary>
        /// Найти роль по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("FindByName/{name}")]
        public async Task<Role> FindByNameAsync(string name) => await _RoleStore.FindByNameAsync(name.ToUpper());

        /// <summary>
        /// Изменить название роли 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("SetRoleName/{name}")]
        public async Task<string> SetRoleNameAsync(Role role, string name)
        {
            await _RoleStore.SetRoleNameAsync(role, name);
            await _RoleStore.SetNormalizedRoleNameAsync(role, name.ToUpper());
            await _RoleStore.UpdateAsync(role);
            return role.Name;
        }

        /// <summary>
        /// Изменить нормализованное название роли 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
        {
            await _RoleStore.SetNormalizedRoleNameAsync(role, name);
            await _RoleStore.UpdateAsync(role);
            return role.NormalizedName;
        }

        /// <summary>
        /// Получить id роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] Role role) => await _RoleStore.GetRoleIdAsync(role);

        /// <summary>
        /// Получить название роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] Role role) => await _RoleStore.GetRoleNameAsync(role);

        /// <summary>
        /// Получить нормализованное название роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) => await _RoleStore.GetNormalizedRoleNameAsync(role);
    }
}
