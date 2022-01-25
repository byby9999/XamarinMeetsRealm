using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    public class Task : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }
        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
        [MapTo("isComplete")]
        public bool IsComplete { get; set; }
        [MapTo("summary")]
        [Required]
        public string Summary { get; set; }
    }
}
