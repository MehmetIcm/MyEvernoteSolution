using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //Creating Articulate Relational Datas
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //FluentAPI
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Comments) //Note Entity has many relational with comments 
                .WithRequired(c => c.Note) //a Comment has to be with Note
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes) 
                .WithRequired(l => l.Note)
                .WillCascadeOnDelete(true);
        }
    }
}