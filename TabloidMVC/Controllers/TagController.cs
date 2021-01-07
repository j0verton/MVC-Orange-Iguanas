using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
        [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;
        public TagController(ITagRepository tagRepository, IPostRepository postRepository)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
        }

        [Authorize(Roles = "Admin")]
        // GET: TagController
        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        [Authorize(Roles = "Admin")]
        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }
        [Authorize(Roles = "Admin")]
        // GET: TagController/Create
        public ActionResult Create()
        {

            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.Add(tag);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);
            return View(tag);
        }
        [Authorize(Roles = "Admin")]
        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.EditTag(tag);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(tag);
            }
        }
        [Authorize(Roles = "Admin, Author")]
        // GET: TagController/AddToPost/5
        public ActionResult AddToPost(int id)
        {
            int userId = GetCurrentUser();
            Post post = _postRepository.GetUserPostById(id, userId);
            //if --- adding a logic here for allowing only on posts
            if (post == null) 
            {
                return RedirectToAction("Details", "Post", new { id = id });
            };
            PostTagViewModel vm = new PostTagViewModel()
            {
                Post = new Post() { Id = id },
                Tags = _tagRepository.GetAllTags(),
                AppliedTags = _tagRepository.GetTagsByPost(id),
                PostTags = _tagRepository.GetPostTagsByPost(id)
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin, Author")]
        // POST: TagController/AddToPost/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToPost(PostTagViewModel vm)
        {
            try
            {
                _tagRepository.AddTagToPost(vm.CurrentTag.Id, vm.Post.Id);
                return RedirectToAction("Details", "Post", new { id = vm.Post.Id });
            }  catch (Exception ex)
            {
                return View(vm);
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _tagRepository.DeleteTag(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Author")]
        public ActionResult Remove(int id, int postId)
        {
            try { 
                _tagRepository.RemoveTagFromPost(id);
                return RedirectToAction("AddToPost", new { id = postId });
            }  catch (Exception ex)
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
