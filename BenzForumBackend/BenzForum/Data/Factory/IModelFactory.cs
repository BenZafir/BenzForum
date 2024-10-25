namespace BenzForum.Data.Factory
{
    /// <summary>
    /// Defines methods for creating instances of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the model to create.</typeparam>
    public interface IModelFactory<T> where T : class, new()
    {
        T Create();
    }
}