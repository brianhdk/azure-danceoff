using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Website.Models;

namespace Website.Controllers
{
    public class LoginController : Controller
    {
	    private readonly UserManager<User> _manager = new UserManager<User>(new NonStoringUserStore()); 

	    [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult External(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalCallback", "Login", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

	        if (loginInfo == null)
		        return RedirectToAction("Index");

			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

			ClaimsIdentity identity = await _manager.CreateIdentityAsync(new User(loginInfo), DefaultAuthenticationTypes.ApplicationCookie);

	        AuthenticationManager.SignIn(new AuthenticationProperties(), identity);

	        return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
	        private readonly string _provider;
	        private readonly string _redirectUri;

	        public ChallengeResult(string provider, string redirectUri)
            {
                _provider = provider;
                _redirectUri = redirectUri;
            }

            public override void ExecuteResult(ControllerContext context)
            {
				var properties = new AuthenticationProperties { RedirectUri = _redirectUri };

				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, _provider);
            }
        }

        #endregion
    }
}