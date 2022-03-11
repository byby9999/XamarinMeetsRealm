using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

namespace App1.Models.GenesisDbModels
{
    public class Procedure : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public bool? SpecificBodySideRequired { get; set; }
        
        [Required]
        public string TenantId { get; set; }
    }
}
