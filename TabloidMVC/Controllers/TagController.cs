﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
        [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
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

        public ActionResult Delete(int id)
        {
            _tagRepository.DeleteTag(id);
            return RedirectToAction("Index");
        }

        // POST: TagController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        _tagRepository.DeleteTag(id);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View("Index");
        //    }
        //}
    }
}
