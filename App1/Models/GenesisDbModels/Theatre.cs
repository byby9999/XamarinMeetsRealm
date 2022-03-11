using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models.GenesisDbModels
{
    public class Theatre : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
        [Required]
        public string TenantId { get; set; }
    }
}
