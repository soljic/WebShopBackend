using Application.Core;
using Application.Dtos;
using MediatR;

namespace Application.MovieDate.Command;

public class CreateMovieDateCommand : IRequest<Result<MovieDateDto>>
{
    public MovieDateDto MovieDateDto { get; set; }

    public CreateMovieDateCommand(MovieDateDto movieDateDto)
    {
        MovieDateDto = movieDateDto;
    }
}