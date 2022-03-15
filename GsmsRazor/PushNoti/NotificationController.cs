using GSMS.API.PRM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GSMS.API.PRM.Controllers
{
    public class NotificationController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> OnPostSendNotification(NotificationModel notificationModel)
        {
            var result = await _notificationService.SendNotification(notificationModel);
            return new JsonResult(result);
        }
    }
}
