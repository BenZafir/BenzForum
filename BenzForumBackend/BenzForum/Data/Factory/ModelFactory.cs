namespace BenzForum.Data.Factory
{
    public class ModelFactory<T> : IModelFactory<T> where T : class, new()
    {
        public T Create()
        {
            return new T();
        }

        public T Create(params object[] parameters)
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not create an instance of {typeof(T).Name} with the provided parameters.", ex);
            }
        }
    }
}


