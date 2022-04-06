﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Mangos.Mvc.Utils.Providers
{
    /// <summary>
    /// From: https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/ModelBinding/QueryStringValueProviderFactory.cs
    /// </summary>
    public class MangosQueryStringValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var query = context.ActionContext.HttpContext.Request.Query;
            if (query != null && query.Count > 0)
            {
                var valueProvider = new QueryStringValueProvider(
                    BindingSource.Query,
                    query,
                    CultureInfo.CurrentCulture); // única linha modificada (de InvariantCulture para CurrentCulture)

                context.ValueProviders.Add(valueProvider);
            }

            return Task.CompletedTask;
        }
    }
}