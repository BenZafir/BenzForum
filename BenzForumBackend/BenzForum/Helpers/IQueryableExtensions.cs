using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BenzForum.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> query) where T : class
        {
            var entityType = typeof(T);
            var navigationProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => (typeof(IEnumerable<>).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string)) ||
                            (!p.PropertyType.IsValueType && p.PropertyType != typeof(string)))
                .Select(p => p.Name);

            foreach (var navigationProperty in navigationProperties)
            {
                try
                {
                    query = query.Include(navigationProperty);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    throw new InvalidOperationException($"Could not include navigation property '{navigationProperty}'", ex);
                }
            }

            return query;
        }
    }
}

