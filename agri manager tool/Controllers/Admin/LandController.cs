using Application.Interfaces;
using Application.UseCases.Land.Commands;
using Application.UseCases.Land.Queries;
using Application.UseCases.Lands.Commands;
using Domain.Enumerables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace agri_manager_tool.Controllers.Admin
{
    [Route("api/Admin/Land")]
    [ApiController]
    public class AdminLandController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext dbContext;
        private readonly ClaimsPrincipal claimsPrincipal;
        public AdminLandController(IMediator mediator, IApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            this.mediator = mediator;
            this.claimsPrincipal = claimsPrincipal;
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddEditLand.Command command)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(AddEditLand.Command command)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                var command = new DeleteLand.Command(Id);
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            try
            {
                var query = new GetLands.Query();
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            try
            {
                var query = new GetLandById.Query(Id);
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpGet("Nearest")]
        public async Task<IActionResult> GetNearestLand([FromQuery] GetNearestLandQuery query)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            try
            {
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
