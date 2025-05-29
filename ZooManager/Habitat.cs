using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZooManager
{
    public class Habitat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; } // Changed from AnimalType to Type to match XAML
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; } // Changed from CurrentAnimals to CurrentOccupancy
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime LastCleanup { get; set; } // Changed from LastCleaning to LastCleanup
        public DateTime NextScheduledCleanup { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } // Active, Maintenance, Closed
        public List<string> ActivityLog { get; set; }

        public Habitat()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = string.Empty;
            Type = string.Empty;
            Capacity = 0;
            CurrentOccupancy = 0;
            Temperature = 20.0;
            Humidity = 50.0;
            LastCleanup = DateTime.Now.AddDays(-1);
            NextScheduledCleanup = DateTime.Now.AddDays(1);
            IsActive = true;
            Status = "Active";
            ActivityLog = new List<string>();
        }

        public Habitat(string name, string type, int capacity, int currentOccupancy)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = name;
            Type = type;
            Capacity = capacity;
            CurrentOccupancy = currentOccupancy;
            Temperature = 20.0;
            Humidity = 50.0;
            LastCleanup = DateTime.Now.AddDays(-1);
            NextScheduledCleanup = DateTime.Now.AddDays(1);
            IsActive = true;
            Status = "Active";
            ActivityLog = new List<string>();
        }

        public override string ToString()
        {
            return $"{Name} - {Type} ({CurrentOccupancy}/{Capacity})";
        }
    }
}
