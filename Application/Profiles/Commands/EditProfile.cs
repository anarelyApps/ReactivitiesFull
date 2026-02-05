
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Application.Profiles.Commands;

public class EditProfile
{
    public class Command: IRequest<Result<Unit>>
    {
        public required string UserId { get; set; }
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var User = await context.Users.FirstOrDefaultAsync(x=>x.Id==request.UserId,cancellationToken);            
            if(User == null) return Result<Unit>.Failure("Cannot find user",400);
            
           User.DisplayName=request.DisplayName;
           User.Bio=request.Bio;

            var result = await context.SaveChangesAsync(cancellationToken)>0;
            return result?Result<Unit>.Success(Unit.Value):Result<Unit>.Failure("Problem updating profile",400);
        }
    }
}
