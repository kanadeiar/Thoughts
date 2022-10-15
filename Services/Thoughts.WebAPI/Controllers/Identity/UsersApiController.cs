using System.Security.Claims;

using DTO.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL;
using Thoughts.DAL.Entities.Idetity;
using Thoughts.Identity.DAL;

using WebStore.Interfaces.Services;

namespace Webstore.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(WebAPIAddresses.Addresses.Identity.Users)]
    public class UsersApiController : ControllerBase
    {
        private readonly UserManager<IdentUser> _UserManager;
        private readonly ThoughtsDB _thoughtsDB;
        private readonly ILogger<UsersApiController> _Logger;

        public UsersApiController(IdentityDB db, ThoughtsDB thoughtsDB, UserManager<IdentUser> userManager, ILogger<UsersApiController> Logger)
        {
            _Logger = Logger;
            _UserManager = userManager;
            _thoughtsDB = thoughtsDB;
        }

        /// <summary>
        /// Получить всех пользователей 
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<IdentUser>> GetAll() => await _UserManager.Users.ToArrayAsync();

        #region Users
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("User")]
        public async Task<bool> CreateAsync([FromBody] IdentUser user)
        {
            var creation_result = await _UserManager.CreateAsync(user);

            if (!creation_result.Succeeded)
                _Logger.LogWarning("Ошибка создания пользователя {0}:{1}",
                    user,
                    string.Join(", ", creation_result.Errors.Select(e => e.Description)));

            //по какому идентификатору искать пользователя в ThoughtsDB
            //var userThoughtsDB = _thoughtsDB.Users.SingleOrDefault(t => t.Email == user.Email);

            //if (userThoughtsDB == null)
            //{
            //    _thoughtsDB.Users.Add(new Thoughts.DAL.Entities.User({  NickName = user.UserName,
            //                                                            IdentityUserId = user.Id,

            //                                                         }));
            //}
            //else
            //{
            //    userThoughtsDB.IdentityUserId = user.Id;

            //}
            //_thoughtsDB.SaveChanges();

            return creation_result.Succeeded;
        }

        /// <summary>
        /// Получить id пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("UserId")]
        public async Task<string> GetUserIdAsync([FromBody] IdentUser user) => await _UserManager.GetUserIdAsync(user);

        /// <summary>
        /// Получить имя пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("UserName")]
        public async Task<string> GetUserNameAsync([FromBody] IdentUser user) => await _UserManager.GetUserNameAsync(user);

        /// <summary>
        /// Изменить имя пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("UserName/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] IdentUser user, string name)
        {
            await _UserManager.SetUserNameAsync(user, name);
            await _UserManager.UpdateAsync(user);
            return user.UserName;
        }

        /// <summary>
        /// Изменить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("User")]
        public async Task<bool> UpdateAsync([FromBody] IdentUser user)
        {
            var update_result = await _UserManager.UpdateAsync(user);

            if (!update_result.Succeeded)
                _Logger.LogWarning("Ошибка редактирования пользователя {0}:{1}",
                    user,
                    string.Join(", ", update_result.Errors.Select(e => e.Description)));

            return update_result.Succeeded;
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync([FromBody] IdentUser user)
        {
            var delete_result = await _UserManager.DeleteAsync(user);

            if (!delete_result.Succeeded)
                _Logger.LogWarning("Ошибка редактирования пользователя {0}:{1}",
                    user,
                    string.Join(", ", delete_result.Errors.Select(e => e.Description)));

            return delete_result.Succeeded;
        }

        /// <summary>
        /// Найти пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("User/Find/{id}")]
        public async Task<IdentUser> FindByIdAsync(string id) => await _UserManager.FindByIdAsync(id);

        /// <summary>
        /// Найти пользователя по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("User/Normal/{name}")]
        public async Task<IdentUser> FindByNameAsync(string name) => await _UserManager.FindByNameAsync(name.ToUpper());

        /// <summary>
        /// Назначить роль пользователю
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] IdentUser user, string role)
        {
            await _UserManager.AddToRoleAsync(user, role.ToUpper());
            await _UserManager.UpdateAsync(user);
        }

        /// <summary>
        /// Удалить роль у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpDelete("Role/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] IdentUser user, string role)
        {
            await _UserManager.RemoveFromRoleAsync(user, role.ToUpper());
        }

        /// <summary>
        /// Получить роль пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] IdentUser user) => await _UserManager.GetRolesAsync(user);

        /// <summary>
        /// Проверка роли у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("InRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] IdentUser user, string role) => await _UserManager.IsInRoleAsync(user, role);

        /// <summary>
        /// Получить пользователей с ролью
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<IdentUser>> GetUsersInRoleAsync(string role) => await _UserManager.GetUsersInRoleAsync(role);

        /// <summary>
        /// Изменить пароль пользователя
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        [HttpPost("AddPasswordAsync")]
        public async Task AddPasswordAsync([FromBody] IdentUser user, string password)
        {
            await _UserManager.AddPasswordAsync(user, password);
            await _UserManager.UpdateAsync(user);
        }

        /// <summary>
        /// Проверить наличие пароля у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] IdentUser user) => await _UserManager.HasPasswordAsync(user);

        #endregion

        #region Claims
        /// <summary>
        /// Получить Claim пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] IdentUser user) => await _UserManager.GetClaimsAsync(user);

        /// <summary>
        /// Добавить Claim
        /// </summary>
        /// <param name="ClaimInfo"></param>
        /// <returns></returns>
        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] ClaimDTO ClaimInfo)
        {
            await _UserManager.AddClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
            await _UserManager.UpdateAsync(ClaimInfo.User);
        }

        /// <summary>
        /// Заменить Claim
        /// </summary>
        /// <param name="ClaimInfo"></param>
        /// <returns></returns>
        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO ClaimInfo)
        {
            await _UserManager.ReplaceClaimAsync(ClaimInfo.User, ClaimInfo.Claim, ClaimInfo.NewClaim);
            await _UserManager.UpdateAsync(ClaimInfo.User);
        }

        /// <summary>
        /// Удалить Claim
        /// </summary>
        /// <param name="ClaimInfo"></param>
        /// <returns></returns>
        [HttpPost("RemoveClaim")]
        public async Task RemoveClaimsAsync([FromBody] ClaimDTO ClaimInfo)
        {
            await _UserManager.RemoveClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
        }

        /// <summary>
        /// Получить пользователей с Claim
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost("GetUsersForClaim")]
        public async Task<IList<IdentUser>> GetUsersForClaimAsync([FromBody] Claim claim) =>
            await _UserManager.GetUsersForClaimAsync(claim);

        #endregion

        #region TwoFactor
        /// <summary>
        /// Включена двухфакторная авторизация
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] IdentUser user) => await _UserManager.GetTwoFactorEnabledAsync(user);

        /// <summary>
        /// Установить двухфакторныую авторизацию
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpPost("SetTwoFactor/{enable}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] IdentUser user, bool enable)
        {
            await _UserManager.SetTwoFactorEnabledAsync(user, enable);
            await _UserManager.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        #endregion

        #region Email/Phone
        /// <summary>
        /// Получить Email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] IdentUser user) => await _UserManager.GetEmailAsync(user);

        /// <summary>
        /// Установить Email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("SetEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] IdentUser user, string email)
        {
            await _UserManager.SetEmailAsync(user, email);
            await _UserManager.UpdateAsync(user);
            return user.Email;
        }

        /// <summary>
        /// Email подтвержден?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] IdentUser user) => await _UserManager.IsEmailConfirmedAsync(user);

        /// <summary>
        /// Подтверждить Email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpPost("SetEmailConfirmed/{enable}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] IdentUser user, string token)
        {
            await _UserManager.ConfirmEmailAsync(user, token);
            await _UserManager.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        /// <summary>
        /// Найти пользователя по Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("UserFindByEmail/{email}")]
        public async Task<IdentUser> FindByEmailAsync(string email) => await _UserManager.FindByEmailAsync(email);

        /// <summary>
        /// Получить номер телефона
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] IdentUser user) => await _UserManager.GetPhoneNumberAsync(user);

        /// <summary>
        /// Установить номер телефона
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] IdentUser user, string phone)
        {
            await _UserManager.SetPhoneNumberAsync(user, phone);
            await _UserManager.UpdateAsync(user);
            return user.PhoneNumber;
        }

        /// <summary>
        /// Подтвержден телефон
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] IdentUser user) =>
            await _UserManager.IsPhoneNumberConfirmedAsync(user);

        /// <summary>
        /// Подтвердить телефон
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] IdentUser user, string token, string phoneNumber)
        {
            await _UserManager.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber);
            await _UserManager.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }
        #endregion

    }
}
