using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEvernote.BusinessLayer
{
    //Deleting Relational Datas
    public class CategoryManager:ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();

            //Deleting Notes which relational with Category
            foreach (Note note in category.Notes.ToList())
            {
                //Deleting Likes which relational with Notes
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }

                //Deleting comments which relational with Notes
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }
            return base.Delete(category);
        }
    }
}
