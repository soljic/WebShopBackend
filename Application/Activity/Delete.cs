using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activity
{
    public class Delete
    {
           public class Command :  IRequest<Result<Unit>>
                {
                    public Guid Id { get; set; }
               
                 }            
                
            public class Handler : IRequestHandler<Command, Result<Unit>>
                {
                    private readonly DataContext _context;
                    public Handler(DataContext context)
                    {
                        _context = context;
                    }
        
                    public async Task<Result<Unit>>  Handle(Command request, CancellationToken cancellationToken)
                    { 
                        var delete = await _context.Activities.FindAsync(request.Id);
                        //  if(delete == null) return null;
                        if(delete == null) return null;
                    
                          _context.Remove(delete);
                          //handler logic implementation

                        var succes = await _context.SaveChangesAsync() > 0;
        
                        if(succes) return Result<Unit>.Success(Unit.Value);
                        return Result<Unit>.Failure("Failed to delete activity");
                    }

        }
            }
    }
