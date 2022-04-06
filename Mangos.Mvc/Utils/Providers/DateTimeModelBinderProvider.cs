using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Mangos.Mvc.Utils.Providers
{
    /// <summary>
    /// Adaptado de: https://stackoverflow.com/a/46051222
    /// </summary>
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.BindingInfo?.BindingSource != BindingSource.Query)
                return null;

            if (context.Metadata.ModelType != typeof(DateTime) && context.Metadata.ModelType != typeof(DateTime?))
                return null;

            return new DateTimeModelBinder();
        }
    }
}