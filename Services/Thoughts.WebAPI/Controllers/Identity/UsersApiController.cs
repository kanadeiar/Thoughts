using System.Security.Claims;

using DTO.Identity;

using Identity.DAL;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL;
using Thoughts.DAL.Entities.Idetity;

using WebStore.Interfaces.Services;

namespace Webstore.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(WebAPIAddresses.Addresses.Identity.Users)]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<User, Role, IdentityDB> _UserStore;
        private readonly ThoughtsDB _thoughtsDB;
        private readonly ILogger<UsersApiController> _Logger;

        public UsersApiController(IdentityDB db, ThoughtsDB thoughtsDB, ILogger<UsersApiController> Logger)
        {
            _Logger = Logger;
            _UserStore = new(db);
            _thoughtsDB = thoughtsDB;
        }

        /// <summary>
        /// Получить всех пользователей 
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAll() => await _UserStore.Users.ToArrayAsync();

        #region Users
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("User")]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var creation_result = await _UserStore.CreateAsync(user);

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
        public async Task<string> GetUserIdAsync([FromBody] User user) => await _UserStore.GetUserIdAsync(user);

        /// <summary>
        /// Получить имя пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("UserName")]
        public async Task<string> GetUserNameAsync([FromBody] User user) => await _UserStore.GetUserNameAsync(user);

        /// <summary>
        /// Изменить имя пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("UserName/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string name)
        {
            await _UserStore.SetUserNameAsync(user, name);
            await _UserStore.SetNormalizedUserNameAsync(user, name.ToUpper());
            await _UserStore.UpdateAsync(user);
            return user.UserName;
        }

        /// <summary>
        /// Получить нормализованное имя пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("NormalUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user) => await _UserStore.GetNormalizedUserNameAsync(user);

        /// <summary>
        /// Изменить нормализованное имя пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("NormalUserName/{name}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string name)
        {
            await _UserStore.SetNormalizedUserNameAsync(user, name);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedUserName;
        }

        /// <summary>
        /// Изменить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("User")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var update_result = await _UserStore.UpdateAsync(user);

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
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var delete_result = await _UserStore.DeleteAsync(user);

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
        public async Task<User> FindByIdAsync(string id) => await _UserStore.FindByIdAsync(id);

        /// <summary>
        /// Найти пользователя по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("User/Normal/{name}")]
        public async Task<User> FindByNameAsync(string name) => await _UserStore.FindByNameAsync(name.ToUpper());

        /// <summary>
        /// Назначить роль пользователю
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role)
        {
            await _UserStore.AddToRoleAsync(user, role.ToUpper());
            await _UserStore.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить роль у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpDelete("Role/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role)
        {
            await _UserStore.RemoveFromRoleAsync(user, role.ToUpper());
            await _UserStore.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Получить роль пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user) => await _UserStore.GetRolesAsync(user);

        /// <summary>
        /// Проверка роли у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("InRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role) => await _UserStore.IsInRoleAsync(user, role);

        /// <summary>
        /// Получить пользователей с ролью
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role) => await _UserStore.GetUsersInRoleAsync(role);

        /// <summary>
        /// Получить Hash пароль пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user) => await _UserStore.GetPasswordHashAsync(user);

        /// <summary>
        /// Изменить Hash пароль пользователя
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        [HttpPost("SetPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _UserStore.SetPasswordHashAsync(hash.User, hash.Hash);
            await _UserStore.UpdateAsync(hash.User);
            return hash.User.PasswordHash;
        }

        /// <summary>
        /// Проверить наличие пароля у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user) => await _UserStore.HasPasswordAsync(user);

        #endregion

        #region Claims
        /// <summary>
        /// Получить Claim пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user) => await _UserStore.GetClaimsAsync(user);

        /// <summary>
        /// Добавить Claim
        /// </summary>
        /// <param name="ClaimInfo"></param>
        /// <returns></returns>
        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] ClaimDTO ClaimInfo)
        {
            await _UserStore.AddClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
            await _UserStore.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Заменить Claim
        /// </summary>
        /// <param name="ClaimInfo"></param>
        /// <returns></returns>
        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO ClaimInfo)
        {
            await _UserStore.ReplaceClaimAsync(ClaimInfo.User, ClaimInfo.Claim, ClaimInfo.NewClaim);
            await _UserStore.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить Claim
        /// </summary>
        /// <param name="ClaimInfo"></param>
        /// <returns></returns>
        [HttpPost("RemoveClaim")]
        public async Task RemoveClaimsAsync([FromBody] ClaimDTO ClaimInfo)
        {
            await _UserStore.RemoveClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
            await _UserStore.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Получить пользователей с Claim
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost("GetUsersForClaim")]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody] Claim claim) =>
            await _UserStore.GetUsersForClaimAsync(claim);

        #endregion

        #region TwoFactor
        /// <summary>
        /// Включена двухфакторная авторизация
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user) => await _UserStore.GetTwoFactorEnabledAsync(user);

        /// <summary>
        /// Установить двухфакторныую авторизацию
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpPost("SetTwoFactor/{enable}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _UserStore.SetTwoFactorEnabledAsync(user, enable);
            await _UserStore.UpdateAsync(user);
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
        public async Task<string> GetEmailAsync([FromBody] User user) => await _UserStore.GetEmailAsync(user);

        /// <summary>
        /// Установить Email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("SetEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _UserStore.SetEmailAsync(user, email);
            await _UserStore.SetNormalizedEmailAsync(user, email.ToUpper());
            await _UserStore.UpdateAsync(user);
            return user.Email;
        }

        /// <summary>
        /// Получить нормализованный Email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user) => await _UserStore.GetNormalizedEmailAsync(user);

        /// <summary>
        /// Установить нормализованный Email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("SetNormalizedEmail/{email?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string? email)
        {
            await _UserStore.SetNormalizedEmailAsync(user, email);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        /// <summary>
        /// Email подтвержден?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user) => await _UserStore.GetEmailConfirmedAsync(user);

        /// <summary>
        /// Установить подтвержден/неподтержден Email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [HttpPost("SetEmailConfirmed/{enable}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _UserStore.SetEmailConfirmedAsync(user, enable);
            await _UserStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        /// <summary>
        /// Найти пользователя по Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("UserFindByEmail/{email}")]
        public async Task<User> FindByEmailAsync(string email) => await _UserStore.FindByEmailAsync(email);

        /// <summary>
        /// Получить номер телефона
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user) => await _UserStore.GetPhoneNumberAsync(user);

        /// <summary>
        /// Установить номер телефона
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _UserStore.SetPhoneNumberAsync(user, phone);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        /// <summary>
        /// Подтвержден телефон
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user) =>
            await _UserStore.GetPhoneNumberConfirmedAsync(user);

        /// <summary>
        /// Установить подтвержден/неподтвержден телефон
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion

    }
}
