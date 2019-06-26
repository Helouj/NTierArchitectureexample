﻿using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userID)
        {

            _userId = userID;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }


        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                IQueryable<NoteListItem> query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
                        .Select(e => new NoteListItem
                        {
                            NoteID = e.NoteID,
                            Title = e.Title,
                            CreatedUtc = e.CreatedUtc
                        }
                        );

                return query.ToArray();
            }
        }
    }
}

