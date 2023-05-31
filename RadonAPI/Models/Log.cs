namespace RadonAPI.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; } = new DateTime(
            DateTime.Now.Year,
            DateTime.Now.Month,
            DateTime.Now.Day,
            DateTime.Now.Hour,
            DateTime.Now.Minute,
            DateTime.Now.Second);

        [BsonElement("serialnumber")]
        public string Serialnumber { get; set; }

        [BsonElement("outside")]
        public Outside Outside { get; set;}

        [BsonElement("inside")]
        public Inside Inside { get; set;}

    }

    public class Outside
    {
        [BsonElement("temperature")]
        public double Temperature { get; set; }

        [BsonElement("humidity")]
        public double Humidity { get; set; }
    }

    public class Inside
    {
        [BsonElement("temperature")]
        public double Temperature { get; set; }

        [BsonElement("humidity")]
        public double Humidity { get; set; }

        [BsonElement("radon")]
        public double Radon { get; set; }

        [BsonElement("radon LTA")]
        public double RadonLTA { get; set; }
    }
}
