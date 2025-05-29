using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZooManager
{
    public class Activity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string HabitatId { get; set; }
        public string Type { get; set; } // Cleaning, Feeding, Maintenance, Inspection, etc.
        public DateTime PerformedAt { get; set; }
        public string Description { get; set; }
        public string PerformedBy { get; set; }

        public Activity()
        {
            Id = ObjectId.GenerateNewId().ToString();
            HabitatId = string.Empty;
            Type = string.Empty;
            PerformedAt = DateTime.Now;
            Description = string.Empty;
            PerformedBy = string.Empty;
        }

        public Activity(string habitatId, string type, string description, string performedBy)
        {
            Id = ObjectId.GenerateNewId().ToString();
            HabitatId = habitatId;
            Type = type;
            PerformedAt = DateTime.Now;
            Description = description;
            PerformedBy = performedBy;
        }

        public Activity(string habitatId, string type, string description, string performedBy, DateTime performedAt)
        {
            Id = ObjectId.GenerateNewId().ToString();
            HabitatId = habitatId;
            Type = type;
            PerformedAt = performedAt;
            Description = description;
            PerformedBy = performedBy;
        }

        public override string ToString()
        {
            return $"{Type} - {Description} by {PerformedBy} at {PerformedAt:dd/MM/yyyy HH:mm}";
        }
    }
}
