using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
                var filePath = Path.GetTempFileName();

                if (model.imageFile != null) {
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        System.Console.WriteLine(stream);
                        await model.imageFile.CopyToAsync(stream);
                    }

                    string pathString = @"wwwroot\\images\\User\\" + num.ToString();
                    if(System.IO.Directory.Exists(pathString))
                    {
                        try
                        {
                            System.IO.Directory.Delete(pathString, true);
                        }

                        catch (System.IO.IOException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    System.IO.Directory.CreateDirectory(pathString);

                    string fileName = "userProfilePic.jpg";
                    pathString = System.IO.Path.Combine(pathString, fileName);

                    string sourceFile = filePath;
                    string destinationFile = pathString;
                    System.IO.File.Move(sourceFile, destinationFile);
                }

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
                if (model.imageFile != null) {
                    editingUser.Image = "userProfilePic.jpg";
                }
                _context.SaveChanges();
                return RedirectToAction("DisplayProfile", new { num = num });
            }
            return View("EditProduct", new { num = num });
        }

    }
}