using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Controllers;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, IUserProfileRepository userProfileRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userProfileRepository = userProfileRepository;
        }
        
        // GET: HomeController1
        public ActionResult Index(int id)
        {
            var currentUser = GetCurrentUser();
            List<Comment> comments = _commentRepository.GetCommentsByPostId(id);
            foreach(Comment comment in comments)
            {
                if(comment.UserProfileId == currentUser)
                {
                    comment.CurrentUser = true;
                }
                else
                {
                    comment.CurrentUser = false;
                }
            }
            Post post = _postRepository.GetPublishedPostById(id);

            CommentCreateViewModel vm = new CommentCreateViewModel
            {
                Comments = comments,
                ParentPost = post
            };

            return View(vm);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin, Author")]
        // GET: HomeController1/Create
        public ActionResult Create(int id)
        {
            var currentUser = GetCurrentUser();
            List<Comment> comments = _commentRepository.GetCommentsByPostId(id);
            Post post = _postRepository.GetPublishedPostById(id);
            CommentCreateViewModel vm = new CommentCreateViewModel
            {
                UserId = currentUser,
                Comments = comments,
                ParentPost = post
            };
            return View(vm);
        }
        [Authorize(Roles = "Admin, Author")]
        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreateViewModel vm)
        {


            try
            {
                vm.NewComment.CreateDateTime = DateAndTime.Now;
                _commentRepository.AddComment(vm.NewComment);

                return RedirectToAction("Index", new { id = vm.NewComment.PostId });
            }
            catch
            {
                return View(vm);
            }
        }
        
        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            var user = _userProfileRepository.GetById(GetCurrentUser());
            Comment comment = _commentRepository.GetCommentById(id);
            if(comment.UserProfileId != user.Id && user.UserTypeId != 1)
            {
                return RedirectToAction("Error", "Shared");
            }
            return View(comment);
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            var user = _userProfileRepository.GetById(GetCurrentUser());
            Comment comment = _commentRepository.GetCommentById(id);
            if (comment.UserProfileId != user.Id && user.UserTypeId != 1)
            {
                return RedirectToAction("Error", "Shared");
            }
            return View(comment);
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                int postId = comment.PostId;
                _commentRepository.DeleteComment(id);
                return RedirectToAction("Index", new { id = postId });
            }
            catch
            {
                return View();
            }
        }

        public int GetCurrentUser()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }

    }
}
