using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ZooManager
{
    public class Utilities
    {
        private readonly IMongoCollection<Users> _userCollection;
        private readonly IMongoCollection<Habitat> _habitatCollection;
        private readonly IMongoCollection<Activity> _activityCollection;

        public Utilities()
        {
            var client = new MongoClient("mongodb+srv://tibee:admin@users.xpt9cbr.mongodb.net/?retryWrites=true&w=majority&appName=Users");
            var database = client.GetDatabase("UserCredentials");
            _userCollection = database.GetCollection<Users>("Users");
            _habitatCollection = database.GetCollection<Habitat>("Habitat");
            _activityCollection = database.GetCollection<Activity>("Activity");
        }

        internal Users ValidateLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _userCollection.Find(u => u.Username == username).FirstOrDefault();

            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }

        // User Management
        internal async Task<bool> AddUserAsync(Users user)
        {
            try
            {
                await _userCollection.InsertOneAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal async Task<List<Users>> GetAllUsersAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        internal async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var result = await _userCollection.DeleteOneAsync(u => u.Id == userId);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        // Habitat Management
        internal async Task<bool> AddHabitatAsync(Habitat habitat)
        {
            try
            {
                await _habitatCollection.InsertOneAsync(habitat);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal async Task<List<Habitat>> GetAllHabitatsAsync()
        {
            return await _habitatCollection.Find(_ => true).ToListAsync();
        }

        internal async Task<bool> UpdateHabitatAsync(Habitat habitat)
        {
            try
            {
                var result = await _habitatCollection.ReplaceOneAsync(h => h.Id == habitat.Id, habitat);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        internal async Task<bool> DeleteHabitatAsync(string habitatId)
        {
            try
            {
                var result = await _habitatCollection.DeleteOneAsync(h => h.Id == habitatId);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        // Activity Management
        internal async Task<bool> AddActivityAsync(Activity activity)
        {
            try
            {
                await _activityCollection.InsertOneAsync(activity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal async Task<List<Activity>> GetActivitiesByHabitatAsync(string habitatId)
        {
            return await _activityCollection.Find(a => a.HabitatId == habitatId)
                                           .SortByDescending(a => a.PerformedAt)
                                           .ToListAsync();
        }
    }
}
