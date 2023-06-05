using Application.Interfaces;
using Application.UseCases.Admin.Commands;
using Application.UseCases.Event.Queries;
using Application.UseCases.Events.Commands;
using Application.UseCases.Events.Queries;
using Domain.Enumerables;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Tracing;
using System.Security.Claims;

namespace agri_manager_tool.Controllers.Admin
{
    [Route("api/Admin/Event")]
    [ApiController]
    public class AdminEventController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext dbContext;
        private readonly ClaimsPrincipal claimsPrincipal;
        public AdminEventController(IMediator mediator, IApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            this.mediator = mediator;
            this.claimsPrincipal = claimsPrincipal;
            this.dbContext = dbContext;
        }

        public record EventCommand(int Id, DateTime Schedule, string Title, string Description);

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(EventCommand eventCommand)
        {

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException();
                var command = eventCommand.Adapt<AddEditEvent.Command>();
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("No permission to take this action.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(EventCommand eventCommand)
        {
            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException();

                var command = eventCommand.Adapt<AddEditEvent.Command>();
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("No permission to take this action.");
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

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException();
                var command = new DeleteEvent.Command(Id);
                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("No permission to take this action.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetEvents.Query query)
        {
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException();

                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("No permission to take this action.");
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
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException();

                var query = new GetEventById.Query(Id);
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("No permission to take this action.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("Month/{DateTime}")]
        public async Task<IActionResult> Get(DateTime DateTime)
        {
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException();

                var query = new GetEventPerMonth.Query(DateTime);
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("No permission to take this action.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
