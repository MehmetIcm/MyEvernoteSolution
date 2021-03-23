/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();

        //It's for calling database creator initializer
        public Test()
        {
            List<Category> categories = repo_category.List();
            List<Category> categories_filtered = repo_category.List(x => x.Id>5);
        }

        public void InsertTest()
        {
            int result = repo_user.Insert(new EvernoteUser()
            {
                Name = "test",
                Surname = "testAdmin",
                Email = "testadmin@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "testadm",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "testAdmin"
            });
        }

        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "testadm");
            if (user != null)
            {
                user.Username = "testUpdate";
                int result = repo_user.Update(user);
            }
        }

        public void DeteleTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "testUpdate");
            if (user !=null)
            {
                repo_user.Delete(user);
            }
        }

        //Testing relational datas
        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);
            //When we use mutltiple instance like EvernoteUser, Note, Comment if we don't make singleton pattern it isn't work so we have to say the program, you should make one intance for every DatabaseContext object
            //Singleton, proje çalışırken bir nesnenin sadece bir kopyası olsun başka kopyası olmasın istiyorsak kullanırız.
            //Biz DatabaseContext nesnemizi comment için ayrı user için ayrı note için ayrı oluşturmasın istiyoruz çünkü program farklı instancelarla çalışmaz
            Comment comment = new Comment()
            {
                Text = "This is a test",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "mehmeticm",
                Note = note,
                Owner = user,
            };

            repo_comment.Insert(comment);
        }
    }
}

*/