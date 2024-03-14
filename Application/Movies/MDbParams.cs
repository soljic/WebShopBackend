using Application.Core;

namespace Application.Movies;

public class MDbParams : PagingParams
{
    public string Search { get; set; }
    public string Trending { get; set; }
    public string Popular { get; set; }
    
}