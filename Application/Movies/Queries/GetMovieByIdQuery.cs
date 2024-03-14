using Application.Core;
using Domain;
using MediatR;

namespace Application.Movies.Queries;

public class GetMovieByIdQuery : IRequest<Result<Movie>>
{
    public int Id { get; set; }
}