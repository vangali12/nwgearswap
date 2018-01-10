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
    public class ProductController : Controller
    {
        private ecommerceContext _context;

        public ProductController(ecommerceContext context)
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
        [Route("products")]
        public IActionResult Products()
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            List<Product> ProductList = _context.Products.Include(w => w.Seller).Where(p => p.Quantity > 0).OrderByDescending(ms => ms.UpdatedAt).ToList();
            ViewBag.Products = ProductList;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            return View("Products");
        }

        [HttpGet]
        [Route("product/{num}")]
        public IActionResult DisplayProduct(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            Product CurrentProduct = _context.Products.Include(u => u.Seller).SingleOrDefault(p => p.productid == num);
            ViewBag.CurrentProduct = CurrentProduct;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            return View("DisplayProduct");
        }

        [HttpGet]
        [Route("addProduct")]
        public IActionResult AddProductPage(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            return View("AddProduct");
        }

        [HttpGet]
        [Route("editProduct/{num}")]
        public IActionResult EditProductForm(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            Product Product = _context.Products.Include(w => w.Seller).Include(p => p.Categories).SingleOrDefault(p => p.productid == num);
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.Product = Product;
            ViewBag.NewMessageCount = CheckMessages();
            return View("EditProduct");
        }

// ***************************************** POSTS ***************************************** //

        [HttpPost]
        [Route("createProduct")]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            int? c = HttpContext.Session.GetInt32("current");
            int currentUserId = (int)c;
            if (ModelState.IsValid) {
                int itemNum = _context.Products.Max(w => w.productid) + 1;
                System.Console.WriteLine(itemNum);

                var filePath = Path.GetTempFileName();

                if (model.Image != null) {
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        System.Console.WriteLine(stream);
                        await model.Image.CopyToAsync(stream);
                    }

                    string pathString = @"wwwroot\\images\\Product\\" + itemNum.ToString();
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

                    string fileName = "productPic.jpg";
                    pathString = System.IO.Path.Combine(pathString, fileName);

                    string sourceFile = filePath;
                    string destinationFile = pathString;
                    System.IO.File.Move(sourceFile, destinationFile);
                }
                Product NewProduct = new Product();
                NewProduct.Name = model.Name;
                if (model.Image != null) {
                    NewProduct.Image = "productPic.jpg";
                }
                NewProduct.Description = model.Description;
                NewProduct.categoryid = itemNum;
                NewProduct.Categories.CampHike = model.Categories.CampHike;
                NewProduct.Categories.Climb = model.Categories.Climb;
                NewProduct.Categories.Cycle = model.Categories.Cycle; 
                NewProduct.Categories.Paddle = model.Categories.Paddle;
                NewProduct.Categories.Run = model.Categories.Run;
                NewProduct.Categories.Snow = model.Categories.Snow;
                NewProduct.Categories.Travel = model.Categories.Travel;
                NewProduct.Categories.Men = model.Categories.Men;
                NewProduct.Categories.Women = model.Categories.Women;
                NewProduct.Categories.Kids = model.Categories.Kids;
                NewProduct.Categories.BooksMaps = model.Categories.BooksMaps;
                NewProduct.Categories.ArtPhotography = model.Categories.ArtPhotography;
                NewProduct.Quantity = model.Quantity;
                NewProduct.Price = model.Price;
                NewProduct.CreatedAt = DateTime.Now;
                NewProduct.UpdatedAt = DateTime.Now;
                NewProduct.userid = currentUserId;
                _context.Products.Add(NewProduct);
                _context.SaveChanges();
                return RedirectToAction("DisplayProfile", "Profile", new { num = currentUserId });
            }
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            return View("AddProduct");
        }

        [HttpPost]
        [Route("editProductPost/{num}")]
        public async Task<IActionResult> EditProductPost(int num, ProductEditModel model)
        {
            int? c = HttpContext.Session.GetInt32("current");
            int currentUserId = (int)c;
            if (ModelState.IsValid) {
                var filePath = Path.GetTempFileName();

                if (model.Image != null) {
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        System.Console.WriteLine(stream);
                        await model.Image.CopyToAsync(stream);
                    }

                    string pathString = @"wwwroot\\images\\Product\\" + num.ToString();
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

                    string fileName = "productPic.jpg";
                    pathString = System.IO.Path.Combine(pathString, fileName);

                    string sourceFile = filePath;
                    string destinationFile = pathString;
                    System.IO.File.Move(sourceFile, destinationFile);
                }

                Product editingProduct = _context.Products.Include(p => p.Categories).SingleOrDefault(p => p.productid == num);
                editingProduct.Name = model.Name;
                if (model.Image != null) {
                    editingProduct.Image = "productPic.jpg";
                }
                editingProduct.Description = model.Description;
                editingProduct.Quantity = model.Quantity;
                editingProduct.Categories.CampHike = model.Categories.CampHike;
                editingProduct.Categories.Climb = model.Categories.Climb;
                editingProduct.Categories.Cycle = model.Categories.Cycle; 
                editingProduct.Categories.Paddle = model.Categories.Paddle;
                editingProduct.Categories.Run = model.Categories.Run;
                editingProduct.Categories.Snow = model.Categories.Snow;
                editingProduct.Categories.Travel = model.Categories.Travel;
                editingProduct.Categories.Men = model.Categories.Men;
                editingProduct.Categories.Women = model.Categories.Women;
                editingProduct.Categories.Kids = model.Categories.Kids;
                editingProduct.Categories.BooksMaps = model.Categories.BooksMaps;
                editingProduct.Categories.ArtPhotography = model.Categories.ArtPhotography;
                _context.SaveChanges();
                return RedirectToAction("DisplayProfile", "Profile", new { num = currentUserId });
            }
            return View("EditProductForm", new { num = num });
        }

        [HttpGet]
        [Route("deleteProduct/{num}")]
        public IActionResult DeleteItem(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            int currentUserId = (int)c;
            System.Console.WriteLine("HIT POST");
            if (ModelState.IsValid) {
                Product deletingProduct = _context.Products.Include(p => p.Categories).SingleOrDefault(p => p.productid == num);
                Category deletingCategory = _context.Categories.SingleOrDefault(k => k.categoryid == num);
                if (deletingProduct.Image != null) {
                    string pathString = @"wwwroot\\images\\Product\\" + num.ToString();
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
                }
                _context.Categories.Remove(deletingCategory);
                _context.Products.Remove(deletingProduct);
                _context.SaveChanges();
                return RedirectToAction("DisplayProfile", "Profile", new { num = currentUserId });
            }
            return RedirectToAction("DisplayProfile", "Profile", new { num = currentUserId });
        }

        [HttpPost]
        [Route("products")]
        public IActionResult FilterSearch(ProductSearchModel model)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            List<Product> ProductList = _context.Products.Include(w => w.Seller).Include(p => p.Categories).ToList();
            if (model != null) {
                if (!string.IsNullOrEmpty(model.Name)) {
                    ProductList = ProductList.Where(x => x.Name.ToLower().Contains(model.Name.ToLower())).ToList();
                }
                if (model.minPrice.HasValue) {
                    ProductList = ProductList.Where(x => x.Price >= model.minPrice).ToList();
                }
                if (model.maxPrice.HasValue) {
                    ProductList = ProductList.Where(x => x.Price <= model.maxPrice).ToList();
                }
                if (!string.IsNullOrEmpty(model.Category)) {
                    if (model.Category == "CampHike") {
                        ProductList = ProductList.Where(x => x.Categories.CampHike == true).ToList();
                    }
                    if (model.Category == "Climb") {
                        ProductList = ProductList.Where(x => x.Categories.Climb == true).ToList();
                    }
                    if (model.Category == "Cycle") {
                        ProductList = ProductList.Where(x => x.Categories.Cycle == true).ToList();
                    }
                    if (model.Category == "Paddle") {
                        ProductList = ProductList.Where(x => x.Categories.Paddle == true).ToList();
                    }
                    if (model.Category == "Run") {
                        ProductList = ProductList.Where(x => x.Categories.Run == true).ToList();
                    }
                    if (model.Category == "Snow") {
                        ProductList = ProductList.Where(x => x.Categories.Snow == true).ToList();
                    }
                    if (model.Category == "Travel") {
                        ProductList = ProductList.Where(x => x.Categories.Travel == true).ToList();
                    }
                    if (model.Category == "Men") {
                        ProductList = ProductList.Where(x => x.Categories.Men == true).ToList();
                    }
                    if (model.Category == "Women") {
                        ProductList = ProductList.Where(x => x.Categories.Women == true).ToList();
                    }
                    if (model.Category == "Kids") {
                        ProductList = ProductList.Where(x => x.Categories.Kids == true).ToList();
                    }
                    if (model.Category == "BooksMaps") {
                        ProductList = ProductList.Where(x => x.Categories.BooksMaps == true).ToList();
                    }
                    if (model.Category == "ArtPhotography") {
                        ProductList = ProductList.Where(x => x.Categories.ArtPhotography == true).ToList();
                    }                    
                }
                if (!string.IsNullOrEmpty(model.Region)) {
                    ProductList = ProductList.Where(s => s.Seller.Region == model.Region || s.Seller.Region == null).ToList();
                }
            }
            ProductList = ProductList.OrderByDescending(ms => ms.UpdatedAt).ToList();
            ViewBag.Products = ProductList;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            return View("Products");
        }

    }
}
        