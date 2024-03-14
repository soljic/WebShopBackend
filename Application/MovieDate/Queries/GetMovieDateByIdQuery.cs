using Application.Core;
using Application.Dtos;
using MediatR;

namespace Application.MovieDate.Queries;

public class GetMovieDateByIdQuery : IRequest<Result<MovieDateDto>>
{
    public GetMovieDateByIdQuery(int dateNightId)
    {
        DateNightId = dateNightId;
    }
    
    public int DateNightId { get; set; }
}