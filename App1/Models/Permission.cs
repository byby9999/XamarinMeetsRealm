using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class Permission : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("canRead")]
        public string CanRead { get; set; }

        [MapTo("canWrite")]
        public string CanWrite { get; set; }

        [MapTo("userid")]
        public string Userid { get; set; }
    }
}
