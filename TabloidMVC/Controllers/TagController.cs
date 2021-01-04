using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

    // GET: TagController
        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }
        
        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }

        // GET: TagController/Create
        public ActionResult Create()
        {

            return View();
        }

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

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {

            Tag tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

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

        // GET: TagController/AddToPost/5
        public ActionResult AddToPost(int id)
        {
            PostTagViewModel vm = new PostTagViewModel()
            {
                Post = _postRepository.GetPublishedPostById(id),
                Tags = _tagRepository.GetAllTags()

            };
            return View(vm);

        }

        // POST: TagController/AddToPost/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToPost(int id, Tag tag)
        {
            try
            {
                _tagRepository.AddTagToPost(id, tag.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        public ActionResult Delete(int id)
        {
            _tagRepository.DeleteTag(id);
            return RedirectToAction("Index");
        }

    }
}
