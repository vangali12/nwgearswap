using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ecommerce.Controllers
{
    public class StoreController : Controller
    {
        private ecommerceContext _context;

        public StoreController(ecommerceContext context)
        {
            _context = context;
        }

        public int CheckMessages() {
            int? c = HttpContext.Session.GetInt32("current");
            int currentUserId = (int)c;
            int NewMessages = _context.Messages.Where(m => m.recipientid == currentUserId).Where(mes => mes.Seen == false).ToList().Count;
            return NewMessages;
        }

// ***************************************** GETS ***************************************** //

        [HttpGet]
        [Route("dashboard")]
        public IActionResult DashboardRedirect()
        {   
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            return RedirectToAction("Dashboard", new { num = currentUserId });
        }

        [HttpGet]
        [Route("dashboard/{num}")]
        public IActionResult Dashboard(int num)
        {   
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            User currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            
            List<Product> Products = _context.Products.Include(w => w.Seller).Where(p => p.Quantity > 0).Where(w => w.Seller.Region == currentUser.Region || w.Seller.Region == null).OrderByDescending(ms => ms.UpdatedAt).ToList();
            
            
            if (Products.Count < 5) {
                ViewBag.TopProducts = Products;
            } else {
                ViewBag.TopProducts = Products.GetRange(0,5);
            }

            List<User> Users = _context.Users.Where(w => w.Region == currentUser.Region).Where(u => u.userid != currentUserId).ToList();

            if (Users.Count < 5) {
                ViewBag.NewUsers = Users;
            } else {
                ViewBag.NewUsers = Users.GetRange(0, 5);
            }

            ViewBag.NewMessageCount = CheckMessages();
            return View("Dashboard");
        }   

// ***************************************** POSTS ***************************************** //

        
    
    }
}