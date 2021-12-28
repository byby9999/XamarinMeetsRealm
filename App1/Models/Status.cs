using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class Status : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
        [MapTo("description")]
        public string Description { get; set; }
    }
}
