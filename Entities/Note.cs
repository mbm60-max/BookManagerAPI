using BManagerAPi.Dtos;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace BManagerAPi.Entities
{
    [Table("Notes")]
    public class Note : BaseModel
    {
        [PrimaryKey("id", true)]
        public Guid Id { get; set; }

        [Column("content")]
        public string Content { get; set; }
        
        // Parameterless constructor
        public Note()
        {
            Content = "No content";
        }

        public (Guid Id, string Content) GetDetails()
        {
            return (Id, Content);
        }

        // Constructor with parameters
        public Note(Guid id)
        {
            Id = id;
            Content = "No content";
        }

        internal NoteDto AsDto()
        {
            return new NoteDto
            {
                Id = this.Id,
                Content = this.Content,
            };
        }
    }
}
