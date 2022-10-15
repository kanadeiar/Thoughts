using DTO.Thoughts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thoughts.DAL.Entities.Idetity;
using Thoughts.Identity.DAL.Interfaces;
using WebStore.Interfaces.Services;

namespace Thoughts.WebAPI.Controllers.Identity
{
    [Authorize]
    [ApiController]
    [Route(WebAPIAddresses.Addresses.Identity.Accounts)]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentUser> _userManager;
        private readonly SignInManager<IdentUser> _signInManager;
        private readonly RoleManager<IdentRole> _roleManager;
        private readonly IAuthUtils<IdentUser> _authUtils;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentUser> userManager,
            SignInManager<IdentUser> signInManager,
            RoleManager<IdentRole> roleManager,
            IAuthUtils<IdentUser> authUtils,
            ILogger<AccountController> logger,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authUtils = authUtils;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequestDTO registerUserRequest)
        {
            try
            {
                _logger.LogInformation("Регистрация пользователя {0}", registerUserRequest.Login);

                var user = new IdentUser()
                {
                    UserName = registerUserRequest.Login,
                    Email = registerUserRequest.Email,
                    PhoneNumber = registerUserRequest.PhoneNumber,
                };

                using (_logger.BeginScope("Регистрация пользователя {0}", registerUserRequest.Login))
                {
                    var result = await _userManager.CreateAsync(user, registerUserRequest.Password);
                    await _userManager.AddToRoleAsync(user, IdentRole.Users);
                    await _signInManager.SignInAsync(user, false);

                    _logger.LogInformation("Пользователь {0} успешно зарегистрирован", registerUserRequest.Login);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Пользователя {0} не удалось зарегестрировать", registerUserRequest.Login);
                }
            }
            return BadRequest("Пользователя не удалось зарегистрировать");
        }

        public record LoginModel(string Login, string Password);

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel Model)
        {
            try
            {
                _logger.LogInformation("Авторизация пользователя {0}", Model.Login);

                var user = await _userManager.FindByNameAsync(Model.Login);
                if (user is not null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var signInResult = await _signInManager.PasswordSignInAsync(
                        userName: Model.Login,
                        password: Model.Password,
                        isPersistent: true,
                        lockoutOnFailure: false);

                    if (signInResult.Succeeded)
                    {
                        _logger.LogInformation("Авторизация пользователя {0} успешна", Model.Login);
                        var sessionToken = _authUtils.CreateSessionToken(user, roles);
                        Response.Headers.Add("Authorization", $"Bearer {sessionToken}");
                        return Ok(sessionToken);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Авторизовать пользователя {0} не удалось", Model.Login);
                }
            }

            _logger.LogInformation("Авторизовать пользователя {0} не удалось", Model.Login);

            return BadRequest("Авторизовать пользователя не удалось");
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("Пользователь вышел из системы");
                return Ok("Пользователь вышел из системы");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Не удалось выйти из системы");
                }
                return BadRequest("Не удалось выйти из системы");
            }
        }

        [Authorize(Roles = $"{IdentRole.Administrators}, {IdentRole.Users}")]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var currentUserName = "";
            try
            {
                currentUserName = _contextAccessor.HttpContext.User.Identity.Name;
                var identityUser = await _userManager.FindByNameAsync(currentUserName);

                if (!string.IsNullOrEmpty(currentUserName) || identityUser is not null)
                {
                    _logger.LogInformation("Смена пароля у пользователя {0}", currentUserName);

                    var identityResult = await _userManager.ChangePasswordAsync(
                        identityUser,
                        oldPassword,
                        newPassword);
                    if (identityResult.Succeeded)
                    {
                        _logger.LogInformation("Смена пароля у пользователя {0} успешна", currentUserName);
                        return Ok(identityResult);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при смене пароля у пользователя {0}", currentUserName);
                }
                return BadRequest("Произошла ошибка при смене пароля");
            }

            return BadRequest("Не удалось изменить пароль");
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRoleAsync(string roleName)
        {
            try
            {
                _logger.LogInformation("Создание роли {0}", roleName);

                using (_logger.BeginScope("Создание роли {0}", roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentRole() { Name = roleName });
                    if (result is not null)
                    {
                        _logger.LogInformation("Роль {0} успешно создана", roleName);

                        return Ok(result);
                    }

                    _logger.LogInformation("Создать роль {0} не удалось", roleName);
                    return BadRequest("Создать роль не удалось");
                }

            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при создании роли {0}", roleName);
                }

                return BadRequest("Произошла ошибка при создании роли");
            }

        }

        [Authorize(Roles = IdentRole.Administrators)]
        //[AllowAnonymous]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            try
            {
                _logger.LogInformation("Получение списка ролей");
                using (_logger.BeginScope("Получение списка ролей"))
                {
                    var result = await _roleManager.Roles.ToListAsync();
                    if (result is not null)
                    {
                        _logger.LogInformation("Cписок ролей получен");
                        return Ok(result);
                    }
                }
                _logger.LogInformation("Не удалось получить список ролей");
                return BadRequest("Не удалось получить список ролей");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при запросе ролей");
                }
                return BadRequest("Произошла ошибка при запросе ролей");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation("Получение роли по id {0}", id);
                var result = await _roleManager.FindByIdAsync(id);
                using (_logger.BeginScope("Получение роли по id {0}", id))
                {
                    if (result is not null)
                    {
                        _logger.LogInformation("Получена роль {0}", result.Name);
                        return Ok(result);
                    }
                }
                _logger.LogInformation("Не удалось получить роль по id {0}", id);
                return BadRequest("Не удалось получить роль");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при запросе роли по id {0}", id);
                }
                return BadRequest("Произошла ошибка при запросе роли");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpPost("EditRoleById")]
        public async Task<IActionResult> EditRoleByIdAsync(string id, string roleName)
        {
            try
            {
                _logger.LogInformation("Редактирование роли. Id = {0}", id);
                var foundRole = await _roleManager.FindByIdAsync(id);
                foundRole.Name = roleName;

                using (_logger.BeginScope("Редактирование роли. Id = {0}", id))
                {
                    var result = await _roleManager.UpdateAsync(foundRole);
                    if (result is not null)
                    {
                        _logger.LogInformation("Роль {0} успешно изменена", foundRole.Name);
                        return Ok(result);
                    }
                }
                _logger.LogInformation("Не удалось изменить роль. Id = {0}", id);
                return BadRequest("Не удалось изменить роль");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при редактировании роли. Id = {0}", id);
                }
                return BadRequest("Произошла ошибка при редактировании роли");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpDelete("DeleteRoleById")]
        public async Task<IActionResult> DeleteRoleByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation("Удаление роли. Id = {0}", id);
                using (_logger.BeginScope("Удаление роли. Id = {0}", id))
                {
                    var result = await _roleManager.FindByIdAsync(id);
                    if (result is not null)
                    {
                        var userRole = await _roleManager.DeleteAsync(result);
                        _logger.LogInformation("Роль успешно удалена. Id = { 0}", id);
                        return Ok(userRole);
                    }
                }
                _logger.LogInformation("Не удалось удалит роль. Id = {0}", id);
                return BadRequest("Не удалось удалить роль");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при удалении роли. Id = {0}", id);
                }
                return BadRequest("Произошла ошибка при удалении роли");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRoleAsync(string login, string role)
        {
            try
            {
                _logger.LogInformation("Добавление роли {0} пользователю {1}", role, login);
                var foundUser = await _userManager.FindByNameAsync(login);

                if (await _userManager.IsInRoleAsync(foundUser, role))
                {
                    _logger.LogInformation("Пользователю {0} уже присвоена роль {1}", role, login);
                    return BadRequest("Пользователю уже присвоена роль");
                }

                using (_logger.BeginScope("Добавление роли {0} пользователю {1}", role, login))
                {
                    var roleAddedResult = await _userManager.AddToRoleAsync(foundUser, role);
                    if (roleAddedResult.Succeeded)
                    {
                        _logger.LogInformation("Роль {0} пользователю {1} добавлена", role, login);
                        return Ok(roleAddedResult);
                    }

                }
                _logger.LogInformation("Не удалось добавить роль {0} пользователю {1}", role, login);
                return BadRequest("Не удалось добавить роль пользователю");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при добавлении роли {0} пользователю {1}", role, login);
                }
                return BadRequest("Произошла ошибка при добавлении роли пользователю");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpDelete("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRoleAsync(string login, string role)
        {
            try
            {
                _logger.LogInformation("Удаление роли {0} пользователю {1}", role, login);
                var foundUser = await _userManager.FindByNameAsync(login);

                if (!await _userManager.IsInRoleAsync(foundUser, role))
                {
                    _logger.LogInformation("У пользователю {0} нет роли {1}", role, login);
                    return BadRequest("У пользователю нет роли");
                }

                using (_logger.BeginScope("Добавление роли {0} пользователю {1}", role, login))
                {
                    var roleRemovedResult = await _userManager.RemoveFromRoleAsync(foundUser, role);
                    if (roleRemovedResult.Succeeded)
                    {
                        _logger.LogInformation("Роль {0} удалена у пользователя {1}", role, login);
                        return Ok(roleRemovedResult);
                    }

                }
                _logger.LogInformation("Не удалось удалить роль {0} у пользователя {1}", role, login);
                return BadRequest("Не удалось удалить роль у пользователя");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при удалении роли {0} у пользователя {1}", role, login);
                }
                return BadRequest("Произошла ошибка при удалении роли у пользователя");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpGet("ListOfUserRoles")]
        public async Task<IActionResult> ListOfUserRolesAsync(string login)
        {
            try
            {
                _logger.LogInformation("Получение всех ролей пользователя {0}", login);
                using (_logger.BeginScope("Получение всех ролей пользователя {0}", login))
                {
                    var foundUser = await _userManager.FindByNameAsync(login);
                    if (foundUser is not null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(foundUser);
                        if (userRoles is not null)
                        {
                            _logger.LogInformation("Получены все роли пользователя {0}", login);
                            return Ok(userRoles.ToList());
                        }
                        _logger.LogInformation("Не удалось получить список ролей пользователя {0}", login);
                        return BadRequest("Не удалось получить список ролей");
                    }
                }
                _logger.LogInformation("Пользователя {0} нет в системе", login);
                return BadRequest("Данного пользователя нет в системе");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при получении списка ролей пользователя {0}", login);
                }
                return BadRequest("Произошла ошибка при получении списка ролей пользователя");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpGet("GetUserByLogin")]
        public async Task<IActionResult> GetUserByEmailAsync(string login)
        {
            try
            {
                _logger.LogInformation("Получение пользователя {0}", login);
                using (_logger.BeginScope("Получение пользователя {0}", login))
                {
                    var result = await _userManager.FindByNameAsync(login);
                    if (result is not null)
                    {
                        _logger.LogInformation("Gользователь {0} получен", login);
                        return Ok(result);
                    }
                }
                return BadRequest("Не удалось получить пользователя");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при получении пользователя {0}", login);
                }
                return BadRequest("Произошла ошибка при получении пользователя");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        //[AllowAnonymous]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Получение всех пользователей");
                using (_logger.BeginScope("Получение списка ролей"))
                {
                    var listOfAllUsers = await _userManager.Users.ToListAsync();
                    if (listOfAllUsers is not null)
                    {
                        _logger.LogInformation("Cписок пользователей получен");
                        return Ok(listOfAllUsers);
                    }
                }
                _logger.LogInformation("Не удалось получить список пользователей");
                return BadRequest("Не удалось получить список пользователей");
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при получении пользователей");
                }
                return BadRequest("Произошла ошибка при получении пользователей");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpPost("EditUserByEmail")]
        public async Task<IActionResult> EditUserByEmailAsync(string email, UserRequestDTO editUserRequest)
        {
            try
            {
                _logger.LogInformation("Изменение email у пользоватиеля {0}", editUserRequest.Login);
                var foundUser = await _userManager.FindByEmailAsync(email);
                if (foundUser is null)
                {
                    _logger.LogInformation("Не удалось найти пользователя {0}", editUserRequest.Login);
                    return BadRequest("Не удалось найти пользователя");
                }

                foundUser.Email = editUserRequest.Email;
                foundUser.PhoneNumber = editUserRequest.PhoneNumber;
                foundUser.UserName = editUserRequest.Email;
                using (_logger.BeginScope("Изменение email у пользоватиеля {0}", editUserRequest.Login))
                {
                    var result = await _userManager.UpdateAsync(foundUser);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Email у пользоватиеля {0} изменен", editUserRequest.Login);
                        return Ok(result);
                    }
                    _logger.LogInformation("Не удалось обновить информацию пользователя {0}", editUserRequest.Login);
                    return BadRequest("Не удалось обновить информацию пользователя");
                }

            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при обновлении информации о пользователе {0}", editUserRequest.Login);
                }
                return BadRequest("Произошла ошибка при обновлении информации о пользователе");
            }
        }

        [Authorize(Roles = IdentRole.Administrators)]
        [HttpDelete("DeleteUserByLogin")]
        public async Task<IActionResult> DeleteUserByEmail(string login)
        {
            try
            {
                _logger.LogInformation("Удаление пользователя {0}", login);
                var foundUser = await _userManager.FindByNameAsync(login);
                if (foundUser is null)
                {
                    _logger.LogInformation("Не удалось удалить пользователя {0}", login);
                    return BadRequest("Не удалось удалить пользователя");
                }
                using (_logger.BeginScope("Удаление пользователя {0}", login))
                {
                    var result = await _userManager.DeleteAsync(foundUser);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Пользователь {0} удален", login);
                        return Ok(result);
                    }

                    _logger.LogInformation("Не удалось удалить пользователя {0}", login);
                    return BadRequest("Не удалось удалить пользователя");
                }
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Произошла ошибка при удалении пользователя {0}", login);
                }
                return BadRequest("Произошла ошибка при удалении пользователя");
            }
        }
    }
}
