namespace  BManagerAPi.Dtos{
    public record NodeDto{
        string Id { get; set; }="-";
        int Priority { get; set; }
        string Previous{ get; set; }="-";
        string Next{ get; set; }="-";
    }
}