using System.Web.Mvc;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            // 默认登录账号密码：admin / 123456
            if (username == "admin" && password == "123456")
            {
                // 登录成功，创建表单认证票
                FormsAuthentication.SetAuthCookie(username, false);
                
                // 重定向到返回URL或默认页面
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Computer");
                }
            }
            else
            {
                // 登录失败，显示错误信息
                ModelState.AddModelError("", "用户名或密码错误");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            // 注销登录
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}