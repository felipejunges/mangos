using Mangos.Dominio.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mangos.Mvc.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddFromResult(this ModelStateDictionary modelState, NotificationResult result)
        {
            foreach (var notification in result.Notifications)
            {
                modelState.AddModelError("", notification.Message);
            }
        }
    }
}