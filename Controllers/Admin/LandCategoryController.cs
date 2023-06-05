using Application.Interfaces;
using Application.UseCases.LandCategory.Commands;
using Application.UseCases.LandCategory.Queries;
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
    public class LandCategoryController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IApplicationDbContext dbContext;
        private readonly ClaimsPrincipal claimsPrincipal;
        public LandCategoryController(IMediator mediator, IApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal)
        {
            this.mediator = mediator;
            this.claimsPrincipal = claimsPrincipal;
            this.dbContext = dbContext;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddEditLandCategory.Command command)
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
        public async Task<IActionResult> Edit(AddEditLandCategory.Command command)
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
                var command = new DeleteLandCategory.Command(Id);
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
                var query = new GetLandCategories.Query();
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
                var query = new GetLandCategoryById.Query(Id);
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
