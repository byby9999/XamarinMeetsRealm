using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models.GenesisDbModels
{
    public class Surgeon : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public string Code { get; set; }
        public string Forename { get; set; }
        public string ID { get; set; }
        public string Surname { get; set; }
        [Required]
        public string TenantId { get; set; }
        public string Title { get; set; }
    }
}
