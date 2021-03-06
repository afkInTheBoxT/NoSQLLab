using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoSQLLab.Models
{
    public class Note
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
        [BsonElement("userId")]
        public Guid UserId { get; set; }
        [BsonElement("lastUpd")]
        public DateTime? LastUpdate { get; set; }
    }
    
}