namespace BManagerAPi.Entities{
    public record User{
        public string Email { get; set; }="-";
        public string Password { get; set; }="-";
         internal object AsDto(){
            throw new NotImplementedException();
        }
    }
}