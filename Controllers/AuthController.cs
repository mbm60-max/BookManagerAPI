
using System;
using System.Threading.Tasks;
using BManagerAPi.Dtos;
using BManagerAPi.Entities;
using BManagerAPi.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace BManagerAPi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserOrderService _userOrderService;

        public AuthController(UserService userService, UserOrderService userOrderService)
        {
            _userService = userService;
            _userOrderService = userOrderService;
        }

        [HttpPost("signin")]
public async Task<ActionResult<SignInResponseDto>> SignInAsync([FromBody] SignInDto signInDto)
{
    try
    {
        // Perform user sign-in
        var user = await _userService.SignInAsync(signInDto.Email, signInDto.Password);
        
        // If user is authenticated, return user information
        if (user != null)
        {
            var signInResponse = new SignInResponseDto
            {
                Id = user.Id,
                IsAuthenticated = true
            };
            return Ok(signInResponse);
        }
        else
        {
            // User authentication failed
            return Unauthorized(); // Or return appropriate status code
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}


        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync([FromBody] SignUpDto signUpDto)
        {
            try
            {
                var user = await _userService.SignUpAsync(signUpDto.Email, signUpDto.Password);
                if (user == null)
                {
                    throw new Exception("Failed to create user account.");
                }

                var accountId = user; // Get the user ID from the signed-up user
                var order = new UserOrder
                {
                    Id = accountId
                };
                await _userOrderService.CreateUserOrderAsync(order);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // UpdateUserAsync and DeleteUserAsync methods should be updated similarly...

    }
}

