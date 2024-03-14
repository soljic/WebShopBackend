using Application.Core;
using Application.Interfaces;
using Application.Movies;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.MovieDate.Command;

public class HandleNotificationResponseCommandHandler : IRequestHandler<HandleNotificationResponseCommand, Result<NotificationRequestAnswer>>
{
    private readonly DataContext _context;
    private readonly MovieHttpClient _client;
    private readonly IUserAccessor _userAccessor;
    private readonly IMapper _mapper;
    
    public HandleNotificationResponseCommandHandler(MovieHttpClient client, IUserAccessor userAccessor, DataContext context, IMapper mapper)
    {
        _client = client;
        _userAccessor = userAccessor;
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result<NotificationRequestAnswer>> Handle(HandleNotificationResponseCommand request, CancellationToken cancellationToken)
    {
        string message;
        
        var dateNight = await  _context.DateNights
            .FirstOrDefaultAsync(x => x.Id == request.NotificationResponse.DateNightId);
        
        if(request.NotificationResponse.Accepted)
        {
            message = "Your date request has been accepted";
            dateNight.IsAccepted = request.NotificationResponse.Accepted;
            _context.DateNights.Update(dateNight);
            await _context.SaveChangesAsync();

        }
        else
        {
            message = "Your date request has not been accepted";
        }
       
        var dateResponse = new NotificationRequestAnswer(request.NotificationResponse.Accepted, message, request.NotificationResponse.DateNightId);
       
        return Result<NotificationRequestAnswer>.Success(dateResponse);
    }
}