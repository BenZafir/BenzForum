namespace BenzForum.Data.Factory
{
    /// <summary>
    /// Defines methods for converting models between different types.
    /// </summary>
    public interface IModelConvertor
    {
        TDestination Convert<TSource, TDestination>(TSource source) where TSource : class where TDestination : class, new();
    }
}