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
                string notificationMessage = "";
                if (post.IsAnonymously)
                {
                    post.SenderId = null;
                    notificationMessage = $"You have new question\n \"{post.Question}\"";

                }
                else
                {
                    var sender = _context.Users.SingleOrDefault(u => u.Id == post.SenderId);
                    notificationMessage =
                        sender == null
                            ? $"You have new question\n\"{post.Question}\""
                            : $"{sender.FullName} asked you a new question.\n\"{post.Question}\"";
                }
                _context.Posts.Add(post);
                _context.SaveChanges();
                var notification = new Notification
                {
                    RecieverId = post.RecieverId,
                    SenderId = post.SenderId,
                    DateCreated = DateTime.Now,
                    Message = notificationMessage,
                    PostId = _context.Posts.Max(p => p.Id)
                };
                _context.Notifications.Add(notification);

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
                        Message = postIndb.Reciever.FullName + " answered your question.",
                        PostId = postIndb.Id
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

        [Route("account/notifications")]
        public ActionResult Notifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.Notifications.Where(n => n.RecieverId == userId).Include(n => n.Sender).OrderByDescending(n => n.DateCreated).ToList();
            return View(notifications);
        }


        [Route("account/question/{postId}/{notificationId}")]
        public ActionResult Question(int postId, int notificationId)
        {
            //var userId = User.Identity.GetUserId();
            var post = _context.Posts.Include(p => p.Reciever).SingleOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return HttpNotFound("Post you're looking for isn't exists.");
            }
            var notification = _context.Notifications.Single(n => n.Id == notificationId);
            notification.IsRead = true;
            _context.SaveChanges();

            return View(post);
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