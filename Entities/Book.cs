using BManagerAPi.Dtos;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace BManagerAPi.Entities
{
    [Table("Book")]
    public class Book : BaseModel
    {
        [PrimaryKey("id", true)]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
        
        [Column("totalPages")]
        public int TotalPages { get; set; }

        [Column("pagesRead")]
        public int PagesRead { get; set; }

        [Column("imageRef")]
        public string ImageRef { get; set; } = "-";

        [Column("author")]
        public string Author { get; set; } = "-";

        [Column("createdDate")]
        public DateOnly CreatedDate { get; set; }

        [Column("notesId")]
        public Guid NotesId { get; set; }

        [Column("ownerId")]
        public string OwnerId { get; set; }

        public (Guid Id, string Name, string Author, DateOnly CreatedDate, int TotalPages, int PagesRead, string ImageRef, Guid NotesId,string OwnerId) GetDetails()
        {
            return (Id, Name, Author, CreatedDate, TotalPages, PagesRead, ImageRef,NotesId,OwnerId);
        }
        public Book()
        {
            Id = Guid.NewGuid();
            NotesId = Id;
        }
        internal BookDto AsDto()
        {
            return new BookDto
            {
                Id = this.Id,
                Name = this.Name,
                Author = this.Author,
                CreatedDate = this.CreatedDate,
                TotalPages = this.TotalPages,
                PagesRead = this.PagesRead,
                ImageRef = this.ImageRef
            };
        }
    }
}
