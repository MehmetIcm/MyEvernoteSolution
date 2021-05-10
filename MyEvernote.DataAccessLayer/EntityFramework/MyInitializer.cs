using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;


namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user..
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Mehmet",
                Surname = "ICME",
                Email = "mehmeticme@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "mehmeticme",
                ProfileImageFileName = "profile_picture.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "mehmeticme"
            };

            // Adding standart user..
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Burak",
                Surname = "ICME",
                Email = "burakicme@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "burakicme",
                Password = "654321",
                ProfileImageFileName = "profile_picture.png",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "mehmeticme"
            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = Faker.NameFaker.FirstName(),
                    Surname = Faker.NameFaker.LastName(),
                    Email = Faker.InternetFaker.Email(),
                    ProfileImageFileName = "profile_picture.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };

                context.EvernoteUsers.Add(user);
            }

            context.SaveChanges();

            // User list for using..
            List<EvernoteUser> userlist = context.EvernoteUsers.ToList();

            // Adding fake categories..
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = Faker.CompanyFaker.Name(),
                    Description = Faker.TextFaker.Sentence(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "mehmeticme"
                };

                context.Categories.Add(cat);

                // Adding fake notes..
                for (int k = 0; k < Faker.NumberFaker.Number(5,9); k++)
                {
                    EvernoteUser owner = userlist[Faker.NumberFaker.Number(5, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = Faker.StringFaker.Alpha(25), 
                        Text = Faker.TextFaker.Sentence(),
                        IsDraft = false,
                        LikeCount = Faker.NumberFaker.Number(1, 9),
                        Owner = owner,
                        CreatedOn = Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                    };

                    cat.Notes.Add(note);

                    // Adding fake comments
                    for (int j = 0; j < Faker.NumberFaker.Number(3, 5); j++)
                    {
                        EvernoteUser comment_owner = userlist[Faker.NumberFaker.Number(0, userlist.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = Faker.TextFaker.Sentences(2),
                            Owner = comment_owner,
                            CreatedOn = Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username
                        };

                        note.Comments.Add(comment);
                    }

                    // Adding fake likes..

                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };

                        note.Likes.Add(liked);
                    }

                }

            }

            context.SaveChanges();
        }
    }
}