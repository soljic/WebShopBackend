namespace Domain.Helpers;

public class GenreEqualityComparer : IEqualityComparer<Genre>

{
    public bool Equals(Genre x, Genre y)
    {
        return x.Id == y.Id;
    }

    public int GetHashCode(Genre obj)
    {
        return obj.Id;
    }
}