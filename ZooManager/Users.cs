using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZooManager
{
    public class Users
    {
   
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public Users(string id, string username, string password, bool isAdmin)
        {
            Id = id;
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }

        public Users()
        {
            Id = ObjectId.GenerateNewId().ToString(); // Generate a new ObjectId if not provided
            Username = string.Empty;
            Password = string.Empty;
            IsAdmin = false; // Default value for IsAdmin
        }

        public override string ToString()
        {
            return $"Username: {Username}, IsAdmin: {IsAdmin}";
        }
    }
}
