using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;

namespace ecommerce.Controllers
{
    public class ProfileController : Controller
    {
        private ecommerceContext _context;

        public ProfileController(ecommerceContext context)
        {
            _context = context;
        } 
        
        public int CheckMessages() {
            int? c = HttpContext.Session.GetInt32("current");
            int currentUserId = (int)c;
            int NewMessages = _context.Messages.Where(m => m.recipientid == currentUserId).Where(mes => mes.Seen == false).ToList().Count;
            return NewMessages;
        }

        string[] Regions = {"", "North Cascades", "Central Cascades", "Snoqualmie Pass", "South Cascades", "Mt Rainier", "Olympics", "Puget Sound & Islands", "Northeast Washington", "Southeast Washington"};

// ***************************************** GETS ***************************************** //

        [HttpGet]
        [Route("profile/{num}")]
        public IActionResult DisplayProfile(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);

            List<Product> SellingItems = _context.Products.Include(u => u.Seller).Where(p => p.userid == num).OrderByDescending(item => item.UpdatedAt).ToList();
            ViewBag.SellingItems = SellingItems;

            User Profile = _context.Users.Include(w => w.Interests).SingleOrDefault(u => u.userid == num);

            ViewBag.Profile = Profile;
            ViewBag.NewMessageCount = CheckMessages();
            return View("DisplayProfile");
        }

        [HttpGet]
        [Route("profile/{num}/edit")]
        public IActionResult EditProfilePage(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            ViewBag.currentUser = _context.Users.Include(u => u.Interests).SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            ViewBag.Regions = Regions;
            return View("EditProfile");
        }

// ***************************************** POSTS ***************************************** //

        // [HttpPost]
        // public IActionResult UploadImage(IFormFile file)

        // {
        //     int? c = HttpContext.Session.GetInt32("current");
        //     if (c == null) {
        //         return RedirectToAction("Index", "Home");
        //     }
        //     int currentUserId = (int)c;
        //     if (ModelState.IsValid) 
        //     {
        //         if (file == null || file.ContentType.ToLower().StartsWith("image/")) 
        //         { 
        //             MemoryStream ms = new MemoryStream(); 
        //             file.OpenReadStream().CopyTo(ms); 

        //             System.Drawing.Image image = System.Drawing.Image.FromStream(ms); 

        //             Image imageEntity = new Image();
        //             imageEntity.imageid = currentUserId;
        //             imageEntity.Name = image.Name; 
        //             imageEntity.Data = ms.ToArray(); 
        //             imageEntity.Width = image.Width;
        //             imageEntity.Height = image.Height;
        //             imageEntity.ContentType = image.ContentType;

        //             _context.Images.Add(imageEntity); 
        //             _context.SaveChanges();
        //         }
        //     }
        //     return RedirectToAction("DisplayProfile");
        // }

        [HttpPost]
        [Route("editProfile/{num}")]
        public async Task<IActionResult> EditUser(int num, UserEditModel model, string desc)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            if (ModelState.IsValid) {
                User editingUser = _context.Users.SingleOrDefault(p => p.userid == num);
                editingUser.FirstName = model.FirstName;
                editingUser.LastName = model.LastName;
                editingUser.Description = desc;
                editingUser.City = model.City;
                editingUser.Region = model.Region;
                editingUser.Interests.CampHike = model.Interests.CampHike;
                editingUser.Interests.Climb = model.Interests.Climb;
                editingUser.Interests.Cycle = model.Interests.Cycle; 
                editingUser.Interests.Paddle = model.Interests.Paddle;
                editingUser.Interests.Run = model.Interests.Run;
                editingUser.Interests.Snow = model.Interests.Snow;
                editingUser.Interests.Travel = model.Interests.Travel;
                editingUser.Interests.Men = model.Interests.Men;
                editingUser.Interests.Women = model.Interests.Women;
                editingUser.Interests.Kids = model.Interests.Kids;
                editingUser.Interests.BooksMaps = model.Interests.BooksMaps;
                editingUser.Interests.ArtPhotography = model.Interests.ArtPhotography;
                _context.SaveChanges();
                return RedirectToAction("DisplayProfile", new { num = num });
            }
            return View("EditProduct", new { num = num });
        }
    }
}