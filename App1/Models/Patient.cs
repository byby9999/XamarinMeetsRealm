using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class Patient : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        public string Name { get; set; }

        public string PatientIdentificationNumber { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }
}
