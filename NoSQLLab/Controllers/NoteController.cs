using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using NoSQLLab.Models;
using NoSQLLab.Repositories;

namespace NoSQLLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : Controller
    {
        private readonly NoteRepository noteRepository;
        private readonly UserRepository userRepository;

        public NoteController(NoteRepository noteRepository, UserRepository userRepository)
        {
            this.noteRepository = noteRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Route("notes")]
        [Authorize]
        public IActionResult GetAll()
        {
            var notes = noteRepository.GetAll();
            return Ok(notes);
        }

        [HttpGet]
        [Route("get")]
        [Authorize]
        public IActionResult GetById(string id)
        {
            var note = noteRepository.GetById(Guid.Parse(id));
            if (note == null)
                return BadRequest(new { Error = "Note not exists" });
            return Json(note);
        }

        [HttpGet]
        [Route("getByUserId")]
        [Authorize]
        public IActionResult GetByUserId(string userId)
        {
            var notes = noteRepository.GetByUserId(Guid.Parse(userId));
            return Json(notes);
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public IActionResult Add(Note newNote)
        {
            if (ModelState.IsValid)
            {
                var username = User?.Identity?.Name;
                var user = userRepository.GetByUsername(username);
                newNote.UserId = user.Id;
                newNote.LastUpdate = DateTime.Now;
                return Json(noteRepository.Insert(newNote));
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize]
        public IActionResult Delete(Guid noteId)
        {
            noteRepository.Delete(noteId);
            return Ok();
        }

        [HttpPut]
        [Route("edit")]
        [Authorize]
        public IActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                var noteDb = noteRepository.GetById(note.Id);
                if (noteDb is null)
                {
                    return BadRequest("There isn't note with this id.");
                }

                noteDb.Text = note.Text;
                noteDb.Title = note.Title;
                noteDb.LastUpdate = DateTime.Now;
                noteRepository.Update(note);
                return Ok(noteDb);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("search")]
        [Authorize]
        public IActionResult Search(string query)
        {
            if (ModelState.IsValid)
            {
                var username = User?.Identity?.Name;
                var user = userRepository.GetByUsername(username);
                var result = noteRepository.Search(user.Id, query);

                return Json(result);
            }

            return BadRequest();
        }
    }
}
