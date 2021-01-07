using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        [Authorize(Roles = "Admin, Author")]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var posts = _postRepository.GetAllPosts();
                return View(posts);
            }
            else
            {
                var posts = _postRepository.GetAllPublishedPosts();
                return View(posts);
            }
      
        }

        [Authorize(Roles = "Admin, Author")]
        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            var vm = new PostTagViewModel();
            vm.Post = post;
            vm.Tags= _tagRepository.GetTagsByPost(id);
            return View(vm);
        }
        [Authorize(Roles = "Admin, Author")]
        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }
        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        [Authorize(Roles = "Admin, Author")]
        public IActionResult MyPosts()
        {
            var MyPosts = _postRepository.GetPostByUserId(GetCurrentUserProfileId());
            return View(MyPosts);

        }
        [Authorize(Roles = "Admin, Author")]
        public IActionResult Delete(int id)
        {
            if (User.IsInRole("Admin"))
            {
                Post post = _postRepository.GetPostById(id);
                return View(post);
            }
            else
            { 
                Post post = _postRepository.GetUserPostById(id, GetCurrentUserProfileId());
                if (post == null)
                { 
                    return RedirectToAction("Details", new { id =id })
                }
                return View(post);
            }
        }

        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(id);
                return RedirectToAction(nameof(MyPosts));
            }
            catch(Exception ex)
            {
                return View(post);
            }
        }

        [Authorize(Roles = "Admin, Author")]
        public ActionResult Edit(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            if (post == null || post.UserProfileId != GetCurrentUserProfileId())
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null || post.UserProfileId != GetCurrentUserProfileId())
                {
                    return NotFound();
                }
            }
            var vm = new PostCreateViewModel();
            vm.Post = post;
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [Authorize(Roles = "Admin, Author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostCreateViewModel vm)
        {
            var categoryList = _categoryRepository.GetAll();
            foreach(var category in categoryList)
            {
                if (category.Id == vm.Post.CategoryId)
                {
                    vm.Post.Category = category;
                }
            }
            try
            {
                _postRepository.UpdatePost(vm.Post);
                return RedirectToAction("MyPosts");
            }
            catch(Exception ex)
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
