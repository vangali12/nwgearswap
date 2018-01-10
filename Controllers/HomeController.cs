using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private ecommerceContext _context;

        public HomeController(ecommerceContext context)
        {
            _context = context;
        }

        static int DayNum(DateTime dt)
        {
            DateTime start = DateTime.Parse("01-Dec-2017");
            TimeSpan t = dt - start;
            return (int)t.TotalSeconds;
        }

// ***************************************** GETS ***************************************** //

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("RegisterPage");
        }
        
        [HttpGet]
        [Route("register")]
        public IActionResult RegisterPage()
        {
            return View("Register");
        }
        
        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            return View("Login");
        }

        

// ***************************************** POSTS ***************************************** //

        [HttpPost]
        [Route("registerUser")]
        public IActionResult Register(UserViewModel model)
        {
            User CheckUser = _context.Users.SingleOrDefault(user => user.Email == model.Email);
            if (CheckUser != null) {
                ViewBag.EmailExistReg = "This email already exists. Please login.";
            } else {
                if (ModelState.IsValid) {
                    int userNum = _context.Users.Max(w => w.userid) + 1;

                    User NewUser = new User();
                    NewUser.FirstName = model.FirstName;
                    NewUser.LastName = model.LastName;
                    NewUser.Email = model.Email;
                    NewUser.Password = model.Password;
                    NewUser.Image = null;
                    NewUser.CreatedAt = DateTime.Now;
                    NewUser.UpdatedAt = DateTime.Now;
                    NewUser.Logged = DateTime.Now;
                    NewUser.interestid = userNum;
                    _context.Users.Add(NewUser);
                    _context.SaveChanges();
                    User current = _context.Users.SingleOrDefault(user => user.Email == model.Email);
                    HttpContext.Session.SetInt32("current", current.userid);
                    return RedirectToAction("Dashboard", "Store", new { num = current.userid });
                }
                
            }
            return View("Register");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string loginEmail, string loginPassword)
        {
            User CheckUser = _context.Users.SingleOrDefault(user => user.Email == loginEmail);
            if (CheckUser == null) {
                ViewBag.EmailExist = "This email does not exist. Please register.";
            } else if (CheckUser.Password != loginPassword) {
                ViewBag.PassMatch = "Your password is incorrect.";
            } else {
                User current = _context.Users.SingleOrDefault(user => user.Email == loginEmail);
                HttpContext.Session.SetInt32("current", current.userid);
                current.Logged = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Store", new { num = current.userid });
            }
            return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        
        public IActionResult Error()
        {
            return View();
        }



    }
}
