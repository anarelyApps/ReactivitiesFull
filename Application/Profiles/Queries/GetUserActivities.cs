using System;
using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetUserActivities
{   public class Query : IRequest<Result<List<UserActivityDto>>>
    {
        public string Filter { get; set; }="hosting";
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
        public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
           var query = context.Activities 
                .Where(x=>x.Attendees.Any(a=>a.IsHost && a.UserId==request.UserId))               
                .ProjectTo<UserActivityDto>(mapper.ConfigurationProvider,
                  new {currentUserId=userAccessor.GetUserId()})
                .AsQueryable();

            query = request.Filter switch
            {
                "past" => query.Where(x=>x.Date<DateTime.UtcNow),
                "future" => query.Where(x=>x.Date>=DateTime.UtcNow),
                _ => query
            };

           var activities = await query.ToListAsync(cancellationToken);

           return Result<List<UserActivityDto>>.Success(activities);
        }
    }

}
