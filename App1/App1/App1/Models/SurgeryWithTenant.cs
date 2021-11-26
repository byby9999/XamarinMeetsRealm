using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    public class SurgeryWithTenant : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public SurgeryWithTenant_BodySide BodySide { get; set; }
        public string Duration { get; set; }
        public int? Lines { get; set; }
        public SurgeryWithTenant_Patient Patient { get; set; }
        public SurgeryWithTenant_Procedure Procedure { get; set; }
        public string Reference { get; set; }
        public SurgeryWithTenant_Surgeon Surgeon { get; set; }
        public string TenantId { get; set; }
        public SurgeryWithTenant_Theatre Theatre { get; set; }
        public double? TheatreTotalCost { get; set; }
        [MapTo("_partition")]
        public string Partition { get; set; }

        public SurgeryWithTenant()
        {
            Id = ObjectId.GenerateNewId();
        }
        public SurgeryWithTenant(string partition)
        {
            this.Partition = partition;
            this.TenantId = partition;
            this.Id = ObjectId.GenerateNewId();

            this.Procedure = new SurgeryWithTenant_Procedure { Code = "32926450", Name = "Baclofen Pump Therapy - Test by alex" };
            this.Surgeon = new SurgeryWithTenant_Surgeon { Code = "9000", FirstName = "Ana", LastName = "Popescu - test by alex", Title = "Mrs" };
        }
    }

    public class SurgeryWithTenant_BodySide : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class SurgeryWithTenant_Patient : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }
    public class SurgeryWithTenant_Procedure : EmbeddedObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class SurgeryWithTenant_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
    public class SurgeryWithTenant_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
