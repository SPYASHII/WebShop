using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Models;
using Microsoft.AspNetCore.Identity;
using WebShop.Enums;
using WebShop.Interfaces;

namespace WebShop.Controllers
{
    public class LoginController : Controller
    {
        //HACK: сделать доступ из иньекции в методах
        private readonly ShopDbContext _shopDb;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ISerializer _serializer;
        public LoginController(ShopDbContext shopDb, IPasswordHasher<User> passwordHasher, ISerializer serializer)
        {
            _shopDb = shopDb;
            _passwordHasher = passwordHasher;
            _serializer = serializer;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionItems.User.ToString());
            HttpContext.Session.SetString(SessionItems.Loged.ToString(), "false");

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string login, string password, string headerColor)
        {
            bool loged = false;

            if (login != null && password != null)
            {
                var user = _shopDb.Users.FirstOrDefault(u => u.Login == login);

                if (user != null)
                {
                    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

                    switch (result)
                    {
                        case PasswordVerificationResult.Success:
                            loged = true;
                            break;
                        case PasswordVerificationResult.SuccessRehashNeeded:
                            //HACK: После оптимизации кода добавить сюда рехэш и сохранение пароля
                            string rehashedPassword = _passwordHasher.HashPassword(user, password);
                            user.Password = rehashedPassword;
                            _shopDb.SaveChanges();

                            loged = true;
                            break;
                        default:
                            break;
                    }
                }

                if (loged)
                {
                    var bytesUser = _serializer.Serialize(user);

                    HttpContext.Session.Set(SessionItems.User.ToString(), bytesUser);
                    HttpContext.Session.SetString(SessionItems.Loged.ToString(), "true");
                }
            }

            if (loged && headerColor != null)
            {
                var options = new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };
                Response.Cookies.Append(CookieItems.HeaderBackgroundColor.ToString(), headerColor, options);
            }

            if (loged)
            {
                return RedirectToAction("Catalog", "Home");
            }
            else
                return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(string login, string password)
        {
            if (login != null && password != null)
            {
                var user = new User(login);

                string hashedPassword = _passwordHasher.HashPassword(user, password);

                user.Password = hashedPassword;

                _shopDb.Add(user);

                _shopDb.SaveChanges();

                return RedirectToAction("Login");
            }

            return View();
        }
    }
}
