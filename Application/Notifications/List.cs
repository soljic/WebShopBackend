using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Comments;
using System;
using System.Linq;
using Application.Interfaces;
using Application.Activity;

namespace Application.Notifications
{
    public class List
    {
        public class Query : IRequest<Result<List<NotificationDto>>>
        {
           
        }

        public class Handler : IRequestHandler<Query, Result<List<NotificationDto>>>
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

            public async Task<Result<List<NotificationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = _context.Users
                        .FirstOrDefault(x => x.UserName == _userAccessor.GetUsername());        
                       


                var notifications = await _context.Notifications.Where(x => x.Receiever.Id == user.Id)
                        .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)                   
                        .ToListAsync();

               

                return Result<List<NotificationDto>>.Success(notifications);
            }


        }
    }
}