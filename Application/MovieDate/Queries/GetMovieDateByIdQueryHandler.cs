using Application.Activity;
using Application.Core;
using Application.Dtos;
using Application.Interfaces;
using Application.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.MovieDate.Queries;

public class GetMovieDateByIdQueryHandler : IRequestHandler<GetMovieDateByIdQuery, Result<MovieDateDto>>
{
    private readonly DataContext _context;
    private readonly IUserAccessor _userAccessor;
    private readonly IMapper _mapper;

    public GetMovieDateByIdQueryHandler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
    {
        _context = context;
        _userAccessor = userAccessor;
        _mapper = mapper;
    }

    public async Task<Result<MovieDateDto>> Handle(GetMovieDateByIdQuery request, CancellationToken cancellationToken)
    {
       if(request.DateNightId is 0)  return Result<MovieDateDto>.Failure("DateNightId is required");
       var movieDateDto = await _context.DateNights
           .ProjectTo<MovieDateDto>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(x => x.DateNightId == request.DateNightId);
       if(movieDateDto is null)  return Result<MovieDateDto>.Failure("No date night found");
       
       return Result<MovieDateDto>.Success(movieDateDto);
    }
}