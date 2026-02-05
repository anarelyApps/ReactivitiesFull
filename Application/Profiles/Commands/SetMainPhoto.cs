using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class SetMainPhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var User = await userAccessor.GetUserWithPhotosAsync();
            var photo = User.Photos.FirstOrDefault(x=>x.Id == request.PhotoId);
            if(photo == null) return Result<Unit>.Failure("Cannot find photo",400);
            
           User.ImageUrl = photo.Url;
            var result = await context.SaveChangesAsync(cancellationToken)>0;
            return result?Result<Unit>.Success(Unit.Value):Result<Unit>.Failure("Problem updating photo",400);
        }
    }
}
