using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Caching;
using Application.Core;
using Application.Movies;
using CloudinaryDotNet;
using Domain;   

namespace Application.Order.Queries
{
    public class GetMoviesQuery : IRequest<Result<PagedListApi<Movie>>>
    {
        public MDbParams MDbparams { get; set; }
    }
   
}
