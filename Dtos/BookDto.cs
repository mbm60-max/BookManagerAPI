namespace BManagerAPi.Dtos{
        public record BookDto{
        public Guid Id { get; init; }//only set value during intialisation

        public int TotalPages { get; set; }
        public int PagesRead   { get; set; }
        public string ImageRef { get; set; }="-";

        public string Name { get; set; }="-";
        public string Author { get; set; }="-";
        public DateOnly CreatedDate { get; set; }

    }
}