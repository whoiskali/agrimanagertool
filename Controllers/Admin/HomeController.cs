using Application.Extensions;
using Application.Interfaces;
using Application.UseCases.Admin.Commands;
using Application.UseCases.User.Commands;
using Domain.Enumerables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace agri_manager_tool.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext dbContext;
        private readonly ClaimsPrincipal claimsPrincipal;
        public AdminController(IMediator mediator, IApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            this.mediator = mediator;
            this.claimsPrincipal = claimsPrincipal;
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpPost("ApproveUser/{Id}")]
        public async Task<IActionResult> Registration(int Id)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var command = new ApproveUser.Command(Id);
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
