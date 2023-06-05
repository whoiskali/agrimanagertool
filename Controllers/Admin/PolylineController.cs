using Application.Interfaces;
using Application.UseCases.Polylines.Commands;
using Application.UseCases.Polylines.Queries;
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
    public class PolylinesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext dbContext;
        private readonly ClaimsPrincipal claimsPrincipal;
        public PolylinesController(IMediator mediator, IApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            this.mediator = mediator;
            this.claimsPrincipal = claimsPrincipal;
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddEditPolylines.Command command)
        {

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException("No permission to take this action.");

                var result = await mediator.Send(command);
                tr.Commit();
                return Ok();
            }
            catch (Exception e)
            {
                //return BadRequest(e.Message);
                return BadRequest(e.InnerException);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(AddEditPolylines.Command command)
        {
            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException("No permission to take this action.");

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

            using var tr = dbContext.Database.BeginTransaction();
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException("No permission to take this action.");

                var command = new DeletePolylines.Command(Id);
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
        public async Task<IActionResult> Get([FromQuery] GetPolylines.Query query)
        {
            if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                throw new UnauthorizedAccessException("No permission to take this action.");

            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException("No permission to take this action.");

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
            try
            {
                if (!claimsPrincipal.IsInRole(Usertype.Administrator.ToString()))
                    throw new UnauthorizedAccessException("No permission to take this action.");

                var query = new GetPolylinesById.Query(Id);
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
