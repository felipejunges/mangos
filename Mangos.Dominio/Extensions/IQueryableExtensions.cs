using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mangos.Dominio.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string attribute)
        {
            return ApplyOrdering(query, attribute, "OrderBy");
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string attribute)
        {
            return ApplyOrdering(query, attribute, "ThenBy");
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string attribute, string orderMethodName)
        {
            //
            string[] attributeSplit = attribute.Split(' ');

            if (attributeSplit.Length > 1)
            {
                attribute = attributeSplit[0];

                if (attributeSplit[1] == "DESC")
                    orderMethodName += "Descending";
            }

            //
            try
            {
                Type t = typeof(T);

                var param = Expression.Parameter(t);
                var property = t.GetProperty(attribute);

                if (property is null)
                    return query;

                return query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        orderMethodName,
                        new Type[] { t, property.PropertyType },
                        query.Expression,
                        Expression.Quote(
                            Expression.Lambda(
                                Expression.Property(param, property),
                                param
                            )
                        )
                    )
                );
            }
            catch (Exception) // Probably invalid input, you can catch specifics if you want
            {
                return query; // Return unsorted query
            }
        }
    }
}