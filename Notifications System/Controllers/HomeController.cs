using Microsoft.AspNet.Identity;
using Notifications_System.Models;
using Notifications_System.Models.AskModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Notifications_System.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("home/posts")]
        public ActionResult Account()
        {
            var userId = User.Identity.GetUserId();
            var posts = _context.Posts.Where(u => u.RecieverId == userId).Include(u => u.Sender).OrderByDescending(post => post.DateAsked).ToList();
            return View(posts);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AskBox(Post post)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            post.DateAsked = DateTime.Now;
            if (post.IsAnonymously)
            {
                post.SenderId = null;
            }

            _context.Posts.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}