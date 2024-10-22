using Business.Commands;
using Business.Queries;
using Domain.Transfer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
                return BadRequest("Invalid user ID format.");

            var user = await _mediator.Send(new GetUserByIdQuery(parsedUserId));

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUserId = await _mediator.Send(new CreateUserCommand(userDto.Username, userDto.Password));

            return CreatedAtAction(nameof(GetUserAsync), new { userId = createdUserId }, createdUserId);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(string userId, [FromBody] UserDTO userDto)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
                return BadRequest("Invalid user ID format.");

            if (!ModelState.IsValid || parsedUserId != userDto.Id)
                return BadRequest("User ID mismatch.");

            var updatedUser = await _mediator.Send(new UpdateUserCommand(parsedUserId, userDto));

            if (updatedUser == null)
                return NotFound("User not found.");

            return Ok(updatedUser);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
                return BadRequest("Invalid user ID format.");

            var deleted = await _mediator.Send(new DeleteUserCommand(parsedUserId));

            if (!deleted)
                return NotFound("User not found.");

            return NoContent();
        }
    }
}
