namespace RadonAPI.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Datalogger
    {
        [BsonElement("serialnumber")]
        public string Serialnumber { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("location")]
        public string? Location { get; set; }
    }
}
