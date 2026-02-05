

using Application.Activities.Commands;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Application.Commands;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedList<ActivityDto,DateTime?>>> GetActivities([FromQuery]ActivityParams activityParams) //CancellationToken ct
    {       

        return HandleResult(await Mediator.Send(new GetActivityList.Query{Params = activityParams}));//new GetActivityList.Query(),ct
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetail(string id)
    {       
       return HandleResult(await Mediator.Send(new GetActivityDetails.Query{Id=id}));        
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command{ActivityDto=activityDto}));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "IsActivityHost")]
    public async Task<ActionResult> EditActivity(string id, EditActivityDto activityDto)
    {
        activityDto.Id = id;
       return HandleResult(await Mediator.Send(new EditActivity.Command{ActivityDto=activityDto}));       
    }

    [HttpDelete("{id}")]
     [Authorize(Policy = "IsActivityHost")]
    public async Task<ActionResult> DeleteActivity(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteActivity.Command{Id=id}));
        
    }

    [HttpPost("{id}/attend")]
    public async Task<ActionResult> Attend(string id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command{Id=id}));
    }
}
