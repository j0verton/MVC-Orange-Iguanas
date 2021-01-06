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
    public class ProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepo;

        public ProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepo = userProfileRepository;
        }

        // GET: ProfileController
        public ActionResult Index()
        {
            var profiles = _userProfileRepo.GetAllUserProfiles();
            return View(profiles);
        }

        // GET: ProfileController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile user = _userProfileRepo.GetById(id); 
            return View(user);
        }

        // GET: ProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Deactivate/5
        public ActionResult Deactivate(int id)
        {
            UserProfile userProfile = _userProfileRepo.GetById(id);
            return View(userProfile);
        }

        // POST: ProfileController/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepo.DeactiveUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }
    }
}
