using Microsoft.AspNetCore.Authorization;
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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: CategoryController
        public ActionResult Categories()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.Add(category);

                return RedirectToAction("Categories");
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoryController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Category category = _categoryRepository.getCategorybyId(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.Edit(category);
                return RedirectToAction("Categories");
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoryController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepository.getCategorybyId(id);
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.Delete(id);
                return RedirectToAction("Categories");
            }
            catch
            {
                return View(category);
            }
        }
    }
}
