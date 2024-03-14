using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using AutoMapper;
using Domain;
using FluentValidation;
using Application.Core;

namespace Application.Activity
{
    public class Edit
    {
                public class Command : IRequest<Result<Unit>>
                {
                       public Activities Activity { get; set;}
                }

                 public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x =>x.Activity).SetValidator(new ActivityValidator());
            }
        }
                 public class Handler : IRequestHandler<Command, Result<Unit>>
                {
                    private readonly DataContext _context;
                    
                    private readonly IMapper _mapper;
                    public Handler(DataContext context, IMapper mapper)
                    {
                        _context = context;
                        _mapper = mapper;
                    }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
                    {   

                        var editing = await _context.Activities.FirstOrDefaultAsync(c => c.Id== request.Activity.Id);
                        if(editing == null) return null;
                       
                      
                        _mapper.Map(request.Activity,editing);
                        var succes = await _context.SaveChangesAsync() > 0;
        
                        if(succes) return Result<Unit>.Success(Unit.Value);
                        return Result<Unit>.Failure("Failed to update activity");
        
                    }
        
            }
    }
}