using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    public class Surgery : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        public string SurgeryId { get; set; }

        public string Reference { get; set; }
        public string StartedOn { get; set; }

        [Required]
        public string ProcedureCode { get; set; }
        [Required]
        public string ProcedureName { get; set; }
        public string BodySide { get; set; }
        public string Theatre { get; set; }
        public string SurgeonWithCode { get; set; }
        public string PatientIdentificationNumber { get; set; }
        public string Duration { get; set; }
        public string Lines { get; set; }
        public string SurgeryItemTotalCost { get; set; }
        public string SurgeryItemTotalCostIncludingVat { get; set; }
        public string SurgeryStaffTotalCost { get; set; }
        public string TheatreTotalCost { get; set; }
    }
}
