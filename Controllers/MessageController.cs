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
    public class MessageController : Controller
    {
        private ecommerceContext _context;

        public MessageController(ecommerceContext context)
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
        [Route("messages")]
        public IActionResult Messages()
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            List<Message> AllMessages = _context.Messages.Include(m => m.Sender).Include(mes => mes.Item).Where(m => m.recipientid == currentUserId).OrderByDescending(ms => ms.CreatedAt).ToList();
            ViewBag.AllMessages = AllMessages;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            return View("Messages");
        }

        [HttpGet]
        [Route("message/{num}")]
        public IActionResult DisplayMessage(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            Message CurrentMessage = _context.Messages.Include(u => u.Sender).Include(mes => mes.Item).SingleOrDefault(m => m.messageid == num);
            CurrentMessage.Seen = true;
            _context.SaveChanges();
            ViewBag.CurrentMessage = CurrentMessage;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            ViewBag.NewMessageCount = CheckMessages();
            return View("DisplayMessage");
        }

        [HttpGet]
        [Route("markUnread/{num}")]
        public IActionResult MarkUnread(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            Message CurrentMessage = _context.Messages.Include(u => u.Sender).Include(mes => mes.Item).SingleOrDefault(m => m.messageid == num);
            CurrentMessage.Seen = false;
            _context.SaveChanges();
            return RedirectToAction("Messages");
        }

        [HttpGet]
        [Route("markAsRead/{num}")]
        public IActionResult MarkAsRead(int num)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            Message CurrentMessage = _context.Messages.Include(u => u.Sender).Include(mes => mes.Item).SingleOrDefault(m => m.messageid == num);
            CurrentMessage.Seen = true;
            _context.SaveChanges();
            return RedirectToAction("Messages");
        }

// ***************************************** POSTS ***************************************** //

        [HttpPost]
        [Route("createMessage/{num}")]
        public IActionResult CreateMessage(int num, MessageViewModel model)
        {
            int? c = HttpContext.Session.GetInt32("current");
            if (c == null) {
                return RedirectToAction("Index", "Home");
            }
            int currentUserId = (int)c;
            ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
            int sellerId = _context.Products.Include(i => i.Seller).SingleOrDefault(i => i.productid == num).Seller.userid;
            if (ModelState.IsValid) {
                Message NewMessage = new Message();
                NewMessage.senderid = currentUserId;
                NewMessage.recipientid = sellerId;
                NewMessage.Title = model.Title;
                NewMessage.Content = model.Content;
                NewMessage.productid = num;
                NewMessage.CreatedAt = DateTime.Now;
                NewMessage.UpdatedAt = DateTime.Now;
                _context.Messages.Add(NewMessage);
                _context.SaveChanges();
                return RedirectToAction("Products", "Product");
            } else {
                Product CurrentProduct = _context.Products.Include(u => u.Seller).SingleOrDefault(p => p.productid == num);
                ViewBag.CurrentProduct = CurrentProduct;
                ViewBag.currentUser = _context.Users.SingleOrDefault(u => u.userid == currentUserId);
                return View("DisplayProduct");
            }
            
        }

    }
}