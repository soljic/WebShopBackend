using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Notifications
{
    public class Create
    {
        public class Command : IRequest<Result<NotificationDto>>
        {
            public string Body { get; set; }
            public string UserName { get; set; }
            public string PartnerId { get; set; }
            public int DateNightId { get; set; }
            
            
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<NotificationDto>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<NotificationDto>> Handle(Command request, CancellationToken cancellationToken)
            {

                var userWhoFollowed = await _context.Users
                    .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                
                var userWhoWasFollowed = await _context.Users
                .Include(n => n.Notifications)
                .SingleOrDefaultAsync(x => x.Id == request.PartnerId);
                
                var dateNight = await _context.DateNights
                    .FirstOrDefaultAsync(x => x.AppUserId == userWhoFollowed.Id && x.PartnerId == request.PartnerId);
                var notification = new Notification
                {
                    Author = userWhoFollowed,
                    Receiever = userWhoWasFollowed,
                    Body = request.Body,
                    DateNightId = dateNight.Id
                };

                userWhoWasFollowed.Notifications.Add(notification);
                await _context.Notifications.AddAsync(notification);
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<NotificationDto>.Success(_mapper.Map<NotificationDto>(notification));

                return Result<NotificationDto>.Failure("Failed to add notification");
            }
        }
    }
}