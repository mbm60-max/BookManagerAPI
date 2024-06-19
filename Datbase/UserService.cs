using Supabase;
using Supabase.Gotrue;
using System;
using System.Threading.Tasks;

namespace BManagerAPi.Repositories
{
    public class UserService
    {
        private readonly Supabase.Client _supabase;

        public UserService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<string> SignUpAsync(string email, string password)
        {
            var session = await _supabase.Auth.SignUp(email, password);

            if (session?.User == null)
            {
                throw new Exception("Failed to sign up the user.");
            }

            return session.User.Id.ToString();
        }
public async Task<SignInResponseDto> SignInAsync(string email, string password)
{
    var session = await _supabase.Auth.SignIn(email, password);
    if (session == null)
    {
        throw new Exception("Failed to sign in the user.");
    }
    
    // Get user information from the session or fetch it from Supabase
    var user = _supabase.Auth.CurrentUser; // Assuming you have a method to retrieve user information based on email
    
    if (user == null)
    {
        throw new Exception("User information not found.");
    }
    
    // Create a SignInResponseDto object with the user information
    var signInResponse = new SignInResponseDto
    {
        Id = user.Id,
        IsAuthenticated = true // Assuming authentication was successful
    };

    return signInResponse;
}


        public async Task UpdateUserAsync(string email, string password)
        {
            var attrs = new UserAttributes { Email = email,Password = password };
            var response = await _supabase.Auth.Update(attrs);
            if (response == null)
            {
                throw new Exception("Failed to update the user.");
            }
        }

        public async Task DeleteUserAsync(string email, string password)
        {
            //
            //if (user == null)
            //{
             //   throw new Exception("Failed to delete the user.");
            //}
        }
    }
}
