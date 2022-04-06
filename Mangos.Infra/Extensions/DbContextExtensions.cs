using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mangos.Infra.Extensions
{
    public static class DbContextExtensions
    {
        // Based on: https://medium.com/@paullorica/automatically-truncate-strings-in-an-ef-core-entity-based-on-maxlength-property-93e17ea7cd43
        public static T TruncateStringsBasedOnMaxLength<T>(this DbContext context, T entityObject)
        {
            if (entityObject is null)
                return entityObject;

            var entityTypes = context.Model.GetEntityTypes();
            var properties = entityTypes.First(e => e.Name == entityObject.GetType().FullName).GetProperties().ToDictionary(p => p.Name, p => p.GetMaxLength());

            foreach (var propertyInfo in entityObject.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
            {
                var value = (string?)propertyInfo.GetValue(entityObject);

                if (value is null)
                    continue;

                var maxLenght = properties[propertyInfo.Name];

                if (maxLenght is null)
                    continue;

                var length = int.Parse(maxLenght.ToString()!);

                if (value.Length > length)
                {
                    propertyInfo.SetValue(entityObject, value.Substring(0, length));
                }
            }

            return entityObject;
        }
    }
}