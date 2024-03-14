using Application.Core;
using Application.Dtos;
using Application.Interfaces;
using Application.Movies;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.MovieDate.Command;

public class CreateMovieDateCommandHandler : IRequestHandler<CreateMovieDateCommand, Result<MovieDateDto>>
{
    private readonly DataContext _context;
    private readonly MovieHttpClient _client;
    private readonly IUserAccessor _userAccessor;
    private readonly IMapper _mapper;
    
    public CreateMovieDateCommandHandler(MovieHttpClient client, IUserAccessor userAccessor, DataContext context, IMapper mapper)
    {
        _client = client;
        _userAccessor = userAccessor;
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<MovieDateDto>> Handle(CreateMovieDateCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => 
            x.UserName == _userAccessor.GetUsername());

        var movie = await _client.GetMovieByIdAsync(request.MovieDateDto.MovieId);
        ArgumentNullException.ThrowIfNull(movie);
            
        var movieDateMovie = new MovieDateDto
        {
            MediaType = request.MovieDateDto.MediaType,
            Date = request.MovieDateDto.Date,
            MovieId = request.MovieDateDto.MovieId,
            PartnerId = request.MovieDateDto.PartnerId,
            User =  user
                
        };

        var dateNight = new DateNight()
        {
            Time = movieDateMovie.Date,
            PartnerId = request.MovieDateDto.PartnerId,
            MediaTypeId = request.MovieDateDto.MovieId,
            MediaType = request.MovieDateDto.MediaType,
            AppUserId = user.Id
        };

        var checkMovie = await  _context.DateNights.Where(x =>
                x.AppUserId == user.Id && x.PartnerId == user.PartnerId && x.MediaTypeId == dateNight.MediaTypeId)
            .FirstOrDefaultAsync();
        
        if(checkMovie is not null) return Result<MovieDateDto>.Failure("There is already a date for this movie");

        _context.DateNights.Add(dateNight);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return Result<MovieDateDto>.Failure("Failed to create Date");

        return Result<MovieDateDto>.Success(movieDateMovie);
    }
}