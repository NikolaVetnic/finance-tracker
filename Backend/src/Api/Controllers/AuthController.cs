using Application.Users.Commands;
using Domain.Entities.Users;
using Domain.Entities.Users.Models;
using Domain.Entities.Users.ValueObjects;
using Domain.Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Cqrs.Abstractions;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var result = await mediator.SendAsync<RegisterUserCommand, RegisterUserResult>(
            new RegisterUserCommand { Username = request.Username, Password = request.Password });

        if (result is null)
            return BadRequest(ELocAuthController.UserAlreadyExists.ToString());

        return Ok(result.User);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
    {
        var result = await mediator.SendAsync<LoginUserCommand, LoginUserResult>(
            new LoginUserCommand { Username = request.Username, Password = request.Password });

        if (result is null)
            return BadRequest(ELocAuthController.InvalidCredentials.ToString());

        return Ok(result.TokenResponse);
    }

    [HttpPost("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        if (!UserId.TryParse(request.UserId, out var userId))
            return Unauthorized(ELocAuthController.InvalidUserId.ToString());
        
        var result = await mediator.SendAsync<RefreshTokenCommand, RefreshTokenResult>(
            new RefreshTokenCommand { UserId = userId!, RefreshToken = request.RefreshToken });

        if (result is null)
            return Unauthorized(ELocAuthController.InvalidRefreshToken.ToString());

        return Ok(result.TokenResponse);
    }

    [Authorize]
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> UpdateUser(UserUpdateDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized(ELocAuthController.UserIdNotFound.ToString());

        if (!UserId.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized(ELocAuthController.InvalidUserId.ToString());

        var result = await mediator.SendAsync<UpdateUserCommand, UpdateUserResult>(
            new UpdateUserCommand { UserId = userId!, Password = request.Password });

        if (result == null)
            return BadRequest(ELocAuthController.UpdateFailedOrUsernameExists.ToString());

        return Ok(result.User);
    }

    [Authorize]
    [HttpGet("Test")]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Test/Admin")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are an admin");
    }
}