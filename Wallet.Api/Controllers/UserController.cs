using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Wallet.BLL.Logic.Contracts.Users;
using Wallet.BLL.Logic.Users;
using Wallet.Common.Entities.User.DB;
using Wallet.Common.Entities.User.InputModels;
using Wallet.Common.Entities.UserModels.InputModels;

namespace Wallet.Api.Controllers
{
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UserCreateInputModel), (int)HttpStatusCode.BadRequest)]

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [AllowAnonymous]
        [HttpGet("{login}/{password}")]
        public async Task<IActionResult> LoginAsync(string login, string password)
        {
            try
            {
                var _userRequest = new UserCreateInputModel();
                _userRequest.Login = login;
                _userRequest.Password = password;
                //создать токен
                var _token = await _userLogic.LoginAsync(_userRequest);

                Response.Cookies.Append("wallet-sec-cookies", _token, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(5),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                return Ok(_token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<UserController>
        [Authorize]
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userLogic.Get();
        }

        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost]
        public async Task PostAsync([FromBody] UserCreateInputModel user)
        {
            await _userLogic.CreateUserAsync(user);
        }


        // GET api/<UserController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<User> Get(Guid id)
        {
            return await _userLogic.Get(id);
        }

        // PUT api/<UserController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task PutAsync(Guid id, [FromBody] UserCreateInputModel user)
        {
            var request = new UserUpdateInputModel
            {
                Id = id,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname, 
            };

            await _userLogic.UpdateUserAsync(request);
        }

        // DELETE api/<UserController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
