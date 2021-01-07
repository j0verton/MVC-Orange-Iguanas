using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class ProfileController : Controller
    {
        private string GetCurrentUserEmail() 
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            return email;
        }

        private readonly IUserProfileRepository _userProfileRepo;

        public ProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepo = userProfileRepository;
        }

        // GET: ProfileController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var profiles = _userProfileRepo.GetAllActiveUserProfiles();
            return View(profiles);
        }

        // GET: ProfileController/Details/5
        [Authorize(Roles = "Admin, Author")]
        public ActionResult Details(int id)
        {
            if (GetCurrentUserProfileId() == id || User.IsInRole("Admin"))
            {
                UserProfile user = _userProfileRepo.GetById(id);
                return View(user);
            }
            else return UserProfile();
        }
        // GET: ProfileController/Details
        [Authorize(Roles = "Admin, Author")]
        public ActionResult UserProfile()
        {
            UserProfile user = _userProfileRepo.GetByEmail(GetCurrentUserEmail());
            return View(user);
        }

        // GET: ProfileController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfileController/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UserProfile user = _userProfileRepo.GetById(id);
            return View(user);
        }

        // POST: ProfileController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile user)
        {
            try
            {
                //make this a list of admins then throw and exception if its 1
                int AdminCount = _userProfileRepo.GetAllActiveUserProfiles().Where(user => user.UserTypeId == 1).Count();

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
        [Authorize(Roles = "Admin")]
        public ActionResult Deactivate(int id)
        {
            UserProfile userProfile = _userProfileRepo.GetById(id);
            return View(userProfile);
        }

        // POST: ProfileController/Deactivate/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, UserProfile userProfile)
        {
            int AdminCount = _userProfileRepo.GetAllActiveUserProfiles().Where(user => user.UserTypeId == 1).Count();
            UserProfile user = _userProfileRepo.GetById(id);
            if (AdminCount == 1 && user.UserTypeId == 1)
            {
                user.UserTypeId = 3;
                //ModelState.AddModelError("UserTypeId", "System must contain 1 active Admin, please add a new Admin before removing");
                return RedirectToAction("Edit", user);
            }
            try
            {
                _userProfileRepo.DeactivateUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }
        // POST: ProfileController/Inactive
        [Authorize(Roles = "Admin")]
        public ActionResult Inactive()
        {
            var inactiveProfiles = _userProfileRepo.GetAllInactiveUserProfiles();
            return View(inactiveProfiles);
        }
        // GET: ProfileController/Reactivate/5
        [Authorize(Roles = "Admin")]
        public ActionResult Reactivate(int id)
        {
            UserProfile userProfile = _userProfileRepo.GetById(id);
            return View(userProfile);
        }

        // POST: ProfileController/Deactivate/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepo.ReactivateUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
