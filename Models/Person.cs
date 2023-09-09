using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace metanit.Models
{
    public class Person
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }
}
