using Example.Projects.Contracts.Commands;
using Example.ReadModel;
using Example.ReadModel.Model;
using Example.WebApp.Shared.Projects;
using Example.WebApp.WebApi;
using Microsoft.AspNetCore.Mvc;
using Modular.Framework.Infrastructure.Mediator;

namespace Example.WebApp.Controllers;

// [ApiController]
// [Authorize]
public class ProjectsController(IDataQuery dataQuery,
                                IMediator mediator) : BaseController
{
    [HttpGet("api/projects")]
    public async Task<IEnumerable<GetProjectsRq>> GetProjects()
    {
        return dataQuery.Query<Projects_Project>()
                        .Select(x => new GetProjectsRq()
                                     {
                                         Id          = x.Id,
                                         ProjectName = x.ProjectName,
                                         Description = x.Description,
                                         Status      = x.Status
                                     });
    }

    [HttpPost("api/projects/{projectid}/status")]
    public async Task ChangeStatus(Guid projectid, [FromBody] ChangeProjectStatusRq model)
    {
        await mediator.Handle(new ChangeProjectStatus(projectid, model.StatusCode, String.Empty));
    }
}