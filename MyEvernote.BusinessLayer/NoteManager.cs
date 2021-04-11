using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEvernote.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> repo_note = new Repository<Note>();

        public List<Note> GetAllNotes()
        {
            return repo_note.List();
        }
        
        public IQueryable<Note> GetAllNotesQueryable()
        {
            return repo_note.ListQueryable();
        }
    }
}
