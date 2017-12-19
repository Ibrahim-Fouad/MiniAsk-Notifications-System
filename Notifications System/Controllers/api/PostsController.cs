using Microsoft.AspNet.Identity;
using Notifications_System.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Routing;

namespace Notifications_System.Controllers.api
{
    [Authorize]
    public class PostsController : ApiController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Route("ajax/notifications/count")]
        public HttpResponseMessage GetNotificationsCount()
        {
            var userId = User.Identity.GetUserId();
            var notificationsCount = _context.Notifications.Count(n => n.RecieverId == userId && !n.IsRead);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                count = notificationsCount
            });
        }


        [Route("ajax/notifications")]
        public HttpResponseMessage GetNewNotifications()
        {
            try
            {
                var userId = User.Identity.GetUserId();


                var notifications = _context.Notifications.Where(n => DbFunctions.DiffSeconds(n.DateCreated, DateTime.Now).Value <= 5 && n.RecieverId == userId).Include(n => n.Post).OrderByDescending(n => n.DateCreated).Take(1).ToList();
                while (notifications.Count == 0)
                {
                    Thread.Sleep(2500);
                    notifications = _context.Notifications.Where(n => DbFunctions.DiffSeconds(n.DateCreated, DateTime.Now).Value <= 5 && n.RecieverId == userId).Include(n => n.Post).OrderByDescending(n => n.DateCreated).Take(1).ToList();
                }
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    notifications = notifications[ 0 ]
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    errorMessage = ex.Message
                });
            }
        }

    }
}
