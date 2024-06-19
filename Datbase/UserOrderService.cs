using System;
using System.Threading.Tasks;
using BManagerAPi.Entities;

namespace BManagerAPi.Repositories{
    public class UserOrderService{
        private readonly Supabase.Client _supabase;

        public UserOrderService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }
        public async Task CreateUserOrderAsync(UserOrder userOrder){
            var response = await _supabase.From<UserOrder>().Insert(userOrder);
            if (response == null || response.Models.Count == 0)
            {
                throw new Exception("Failed to insert userOrder into the database.");
            }
        }
        public async Task<UserOrder> GetUserOrderAsync(string id){
          var response = await _supabase
                .From<UserOrder>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString())
                .Single();

            if (response == null)
            {
                throw new Exception("User order not found.");
            }
            return response;  
        }
        public async Task UpdateUserOrderAsync(string id, string orderJsonAsString){
            Console.WriteLine(id);
            Console.WriteLine(orderJsonAsString);   
        var update = await _supabase
            .From<UserOrder>()
            .Where(x => x.Id == id)
            .Set(x => x.OrderJsonAsString, orderJsonAsString)
            .Update();
            if (update == null)
            {
                throw new Exception("Failed to update user order.");
            }
        }
        public async Task DeleteUserOrderAsync(string id){
            try{
                await _supabase.From<UserOrder>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Delete();
            }catch{
                throw new Exception("Failed to delete user order");
            }
        }
    }
}