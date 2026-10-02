using DotNetFlicks.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFlicks.Web.Controllers
{
    [Authorize]
    public class MusicController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IMusicManager _musicManager;

        public MusicController(IAccountManager accountManager, IMusicManager musicManager)
        {
            _accountManager = accountManager;
            _musicManager = musicManager;
        }

        public ActionResult ViewAll()
        {
            var user = _accountManager.GetApplicationUser(HttpContext.User).Result;
            return View(_musicManager.GetAll(user.Id));
        }

        public ActionResult ViewAllForUser()
        {
            var user = _accountManager.GetApplicationUser(HttpContext.User).Result;
            return View("ViewAll", _musicManager.GetAllForUser(user.Id));
        }

        public ActionResult View(int id)
        {
            var user = _accountManager.GetApplicationUser(HttpContext.User).Result;
            return View(_musicManager.Get(id, user.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToLibrary(int id)
        {
            var user = _accountManager.GetApplicationUser(HttpContext.User).Result;
            _musicManager.AddToLibrary(id, user.Id);
            return RedirectToAction("View", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromLibrary(int id, bool returnToLibrary = false)
        {
            var user = _accountManager.GetApplicationUser(HttpContext.User).Result;
            _musicManager.RemoveFromLibrary(id, user.Id);

            return returnToLibrary
                ? RedirectToAction("ViewAllForUser")
                : RedirectToAction("View", new { id });
        }
    }
}
