namespace App1.Models
{
    using MongoDB.Bson;
    using Realms;
    using System;
    using System.Collections.Generic;

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

        public IList<SurgeryWithTenant_SurgeryItems> SurgeryItems { get; }

        public IList<SurgeryWithTenant_SurgeryProcedures> SurgeryProcedures { get; }

        public IList<SurgeryWithTenant_SurgeryStaffs> SurgeryStaffs { get; }

        public string TenantId { get; set; }

        public SurgeryWithTenant_Theatre Theatre { get; set; }

        public double? TheatreTotalCost { get; set; }

        [MapTo("_partition")]
        public string Partition { get; set; }
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
    public class SurgeryWithTenant_SurgeryItems : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public int? Quantity { get; set; }
        public string SerialNumber { get; set; }
    }
    public class SurgeryWithTenant_SurgeryProcedures : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class SurgeryWithTenant_SurgeryStaffs : EmbeddedObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<SurgeryWithTenant_SurgeryStaffs_SurgeryStaffTimings> SurgeryStaffTimings { get; }
        public double? TheatreStaffRate { get; set; }
    }
    public class SurgeryWithTenant_SurgeryStaffs_SurgeryStaffTimings : EmbeddedObject
    {
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartTime { get; set; }
    }
    public class SurgeryWithTenant_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

}
