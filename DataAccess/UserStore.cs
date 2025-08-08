using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using NurseCare.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;

namespace NurseCare.DataAccess
{
    public class UserStore
    {
        private static Dictionary<string, string> _users = new();
        //NurseCareDBContext dBContext = new NurseCareDBContext();
        public UserStore()
        {
            // Initialize the user store if needed, e.g., load existing users from a database
            // For now, we will keep it simple and use an in-memory dictionary
            //dBContext = new NurseCareDBContext();
        }
        public static bool Register(UserLogin userLogin, Nurse nurse,UpdateTeamInfo teamInfo,ContactAddress contactAddress)
        {
            //if (_users.ContainsKey(username)) return false;
            //_users[username] = password;
            if (userLogin == null || nurse == null || teamInfo == null)
            {
                throw new ArgumentNullException("UserLogin, Nurse, or UpdateTeamInfo cannot be null.");
            }
            try
            {
                using(var context = new NurseCareDBContext())
                {                   
                    // Add the new user login
                    context.UserLogins.Add(userLogin);
                    context.Nurses.Add(nurse);
                    context.UpdateTeamInfos.Add(teamInfo);
                    context.ContactAddresses.Add(contactAddress);

                    // Save changes to the database
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error registering user: {ex.Message} {ex.Source}");
                return false;
            }
            return true;
        }

        public static bool Validate(string username, string password)
        {
            // return _users.TryGetValue(username, out var stored) && stored == password;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false; // Invalid credentials
            }
            try { 
            using var dBContext = new NurseCareDBContext();
            return dBContext
                .UserLogins
                .Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
            catch { return false; }
        }
        public static string? GetPersonId(string username,string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null; // Invalid username
            }
            try
            {
                using var dBContext = new NurseCareDBContext();
                return dBContext.UserLogins
                    .Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password)
                    .Select(u => u.PersonId)
                    .FirstOrDefault();
            }
            catch { return null; }
        }
        public static bool IsRegistered(string username)
        {
            using var dBContext = new NurseCareDBContext();
            try
            {
                return dBContext.UserLogins
                    .Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch { return false; }
        }
    }
}
