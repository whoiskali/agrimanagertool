using Application.Extensions;
using Application.Interfaces;
using Application.UseCases.User.Commands;
using Application.UseCases.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace agri_manager_tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext dbContext;
        private readonly ClaimsPrincipal claimsPrincipal;
        public UserController(IMediator mediator, IApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.mediator = mediator;
            this.dbContext = dbContext;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] Registration.Command command)
        {
            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok(result);
            }
            catch (Exception)
            {
                tr.Rollback();
                throw;
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login.Command command)
        {
            try
            {
                var result = await mediator.Send(command);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("Data")]
        public async Task<IActionResult> Data()
        {

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var query = new GetData.Command((int)claimsPrincipal.GetUserId());
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var query = new GetUsers.Query();
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
