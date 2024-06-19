using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace BManagerAPi.Entities{

        [Table("Order")]
    public class UserOrder : BaseModel
    {
        [PrimaryKey("id", true)]
        public string Id { get; set; }

        [Column("orderJson")]
        public string OrderJsonAsString { get; set; }
        
        // Parameterless constructor
        public UserOrder()
        {
            OrderJsonAsString = "No JSON";
        }

        public (string Id, string OrderJsonAsString) GetDetails()
        {
            return (Id, OrderJsonAsString);
        }

        // Constructor with parameters
        public UserOrder(string id)
        {
            Id = id;
            OrderJsonAsString = "No JSON";
        }
    }
}