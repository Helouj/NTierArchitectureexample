using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult Index()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            NoteService service = new NoteService(userId);
            IEnumerable<NoteListItem> model = service.GetNotes();
            //check user's identity, creates a new service with their userid, uses service to get their notes, then returns the list of their notes.  classic repository stuff right here

            return View(model);
        }
        //GET
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
     [ValidateAntiForgeryToken]
     public ActionResult Create(NoteCreate model)
     {
         if (!ModelState.IsValid) return View(model);            

         var service = CreateNoteService();

         if (service.CreateNote(model))
         {
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            };

         ModelState.AddModelError("", "Note could not be created.");

         return View(model);
     }

        private NoteService CreateNoteService()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            NoteService service = new NoteService(userId);
            return service;
        }
    }
}