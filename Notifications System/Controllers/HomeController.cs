using Microsoft.AspNet.Identity;
using Notifications_System.Models;
using Notifications_System.Models.AskModels;
using Notifications_System.ViewModels;
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

        [Route("user/{username}")]
        public ActionResult GetUsername(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.UniqueUsername == username);
            if (user == null)
            {
                return HttpNotFound("The user which you looking for is not exists.");
            }
            var userPosts = _context.Posts.Where(u => u.RecieverId == user.Id && u.DateAnswerd != null).Include(m => m.Sender).Include(m => m.Reciever).OrderByDescending(post => post.DateAsked).ToList();
            var viewModel = new UsernameFormViewModel
            {
                User = user,
                Posts = userPosts
            };
            return View(viewModel);
        }

        [Route("account/posts/new")]
        public ActionResult UnAnswerdQuestion()
        {
            var userId = User.Identity.GetUserId();
            var posts = _context.Posts.Where(u => u.RecieverId == userId && u.DateAnswerd == null).Include(u => u.Sender).OrderByDescending(post => post.DateAsked).ToList();
            return View(posts);
        }


        [Route("account/posts")]
        public ActionResult Account()
        {
            var userId = User.Identity.GetUserId();
            var posts = _context.Posts.Where(u => u.RecieverId == userId && u.DateAnswerd != null).Include(u => u.Sender).Include(u => u.Reciever).OrderByDescending(post => post.DateAnswerd).ToList();
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

            if (post.Id == 0)
            {
                post.DateAsked = DateTime.Now;
                if (post.IsAnonymously)
                {
                    post.SenderId = null;
                }

                _context.Posts.Add(post);
            }
            else
            {
                var postIndb = _context.Posts.SingleOrDefault(u => u.Id == post.Id);
                if (postIndb == null)
                {
                    return HttpNotFound();
                }

                postIndb.DateAnswerd = DateTime.Now;
                postIndb.Answer = post.Answer;
            }


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