namespace BManagerAPi.Dtos{
        public record NoteDto{
        public Guid Id { get; init; }//only set value during intialisation

        public required string Content { get; set; }

    }
}