using Microsoft.AspNet.Identity;
using Notifications_System.Extenstion;
using Notifications_System.Models;
using Notifications_System.Models.AskModels;
using Notifications_System.Models.NotificationModels;
using Notifications_System.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Notifications_System.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Account");
        }

        [AllowAnonymous]
        [Route("user/{username}")]
        public ActionResult GetUsername(string username)
        {
            if (User.Identity.GetUsername() == username)
                return RedirectToAction("Account");


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
                else
                {
                    var sender = _context.Users.SingleOrDefault(u => u.Id == post.SenderId);
                    var notification = new Notification
                    {
                        RecieverId = post.RecieverId,
                        SenderId = post.SenderId,
                        DateCreated = DateTime.Now,
                        Message =
                            sender == null
                                ? "You have new question\n" + post.Question
                                : $"{sender.FullName} has asked you a new question, you can review it.\n" + post.Question
                    };
                    _context.Notifications.Add(notification);
                }
                _context.Posts.Add(post);
            }
            else
            {
                Post postIndb;
                if (post.SenderId != null)
                {
                    postIndb = _context.Posts.Include(u => u.Reciever).SingleOrDefault(u => u.Id == post.Id);
                    if (postIndb == null)
                    {
                        return HttpNotFound();
                    }

                    var notification = new Notification
                    {
                        RecieverId = postIndb.SenderId,
                        SenderId = postIndb.RecieverId,
                        DateCreated = DateTime.Now,
                        Message = postIndb.Reciever.FullName + " has answered your question, you can review it."
                    };
                    _context.Notifications.Add(notification);
                }
                else
                {
                    postIndb = _context.Posts.SingleOrDefault(u => u.Id == post.Id);
                    if (postIndb == null)
                        return HttpNotFound();


                }


                postIndb.DateAnswerd = DateTime.Now;
                postIndb.Answer = post.Answer;

            }


            _context.SaveChanges();
            return RedirectToAction("UnAnswerdQuestion");
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