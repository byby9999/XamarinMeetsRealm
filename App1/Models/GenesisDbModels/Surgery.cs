using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models.GenesisDbModels
{
    public class Surgery : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public string EndTime { get; set; }
        public string GRSN { get; set; }
        public string PatientId { get; set; }
        public Surgery_Procedure Procedure { get; set; }
        public string StartTime { get; set; }
        public string State { get; set; }
        public Surgery_Surgeon Surgeon { get; set; }
        public string SurgeryId { get; set; }
        public string SurgeryScheduleId { get; set; }
        public string SyncState { get; set; }
        [Required]
        public string TenantId { get; set; }
        public Surgery_Theatre Theatre { get; set; }

        public Surgery()
        {

        }
        public Surgery(string name, string partition)
        {
            this.Procedure = new Surgery_Procedure("code", name);
            this.TenantId = partition;
        }
    }

    public class Surgery_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string Forename { get; set; }
        public string ID { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
    }

    public class Surgery_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
    }
    public class Surgery_Procedure : EmbeddedObject
    {
        public string Bodyside { get; set; }
        public string Code { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }

        public Surgery_Procedure()
        {

        }
        public Surgery_Procedure(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
