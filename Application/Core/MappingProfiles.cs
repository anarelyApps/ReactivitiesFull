
using System.Security.Cryptography.X509Certificates;
using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string? currentUserId=null;
        CreateMap<Activity,Activity>();
        CreateMap<CreateActivityDto,Activity>();
        CreateMap<EditActivityDto,Activity>();
        CreateMap<Activity,ActivityDto>()
        .ForMember(d=>d.HostDisplayName,o=>o.MapFrom(s=>
        s.Attendees.FirstOrDefault(x=>x.IsHost)!.User.DisplayName));

        CreateMap<ActivityAttendee,UserProfile>()
        .ForMember(X=>X.DisplayName,o=>o.MapFrom(s=>s.User.DisplayName))
        .ForMember(X=>X.Bio,o=>o.MapFrom(s=>s.User.Bio))
        .ForMember(X=>X.ImageUrl,o=>o.MapFrom(s=>s.User.ImageUrl))
        .ForMember(X=>X.Id,o=>o.MapFrom(s=>s.User.Id))
        .ForMember(d=>d.FollowersCount,o=>o.MapFrom(s=>s.User.Followers.Count))
        .ForMember(d => d.FollowingCount,o=>o.MapFrom(s=>s.User.Followings.Count))
        .ForMember(d=>d.Following,o=>o.MapFrom(s=> 
            s.User.Followers.Any(x=>x.Observer.Id == currentUserId)));
            
       CreateMap<User,UserProfile>()
        .ForMember(d=>d.FollowersCount,o=>o.MapFrom(s=>s.Followers.Count))
        .ForMember(d=>d.FollowingCount,o=>o.MapFrom(s=>s.Followings.Count))
        .ForMember(d=>d.Following,o=>o.MapFrom(s=>
            s.Followers.Any(x=>x.Observer.Id == currentUserId)));

       CreateMap<Comment,CommentDto>()
       .ForMember(d=>d.DisplayName,o=>o.MapFrom(s=>s.User.DisplayName))
       .ForMember(d=>d.UserId,o=>o.MapFrom(s=>s.User.Id))
       .ForMember(d=>d.ImageUrl,o=>o.MapFrom(s=>s.User.ImageUrl));

       CreateMap<Activity,UserActivityDto>();
    }
}
