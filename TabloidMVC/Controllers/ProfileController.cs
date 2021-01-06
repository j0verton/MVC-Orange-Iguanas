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
            UserProfile user = _userProfileRepo.GetById(id);
            return View(user);
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile user)
        {
            try
            {
                //make this a list of admins then throw and exception if its 1
                int AdminCount = _userProfileRepo.GetAllUserProfiles().Where(user => user.UserTypeId == 1).Count();

                if (AdminCount == 1 && user.UserTypeId != 1)
                {
                    ModelState.AddModelError("UserTypeId", "System must contain 1 active Admin, please add a new Admin before removing");
                    return View(user);
                }
                _userProfileRepo.EditUser(user);
                return RedirectToAction("Details", new { id = user.Id});
            }
            catch(Exception ex)
            {
                return View(user);
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
            int AdminCount = _userProfileRepo.GetAllUserProfiles().Where(user => user.UserTypeId == 1).Count();
            UserProfile user = _userProfileRepo.GetById(id);
            if (AdminCount == 1 && user.UserTypeId == 1)
            {
                user.UserTypeId = 3;
                //ModelState.AddModelError("UserTypeId", "System must contain 1 active Admin, please add a new Admin before removing");
                return RedirectToAction("Edit", user);
            }
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
