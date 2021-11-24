using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;

namespace App1.Models
{

    public class SurgeryWithDetail : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        public SurgeryWithDetail_BodySide BodySide { get; set; }

        public string Duration { get; set; }

        public int? Lines { get; set; }

        public SurgeryWithDetail_Patient Patient { get; set; }

        public SurgeryWithDetail_Procedure Procedure { get; set; }

        public string Reference { get; set; }

        public SurgeryWithDetail_Surgeon Surgeon { get; set; }

        public IList<SurgeryWithDetail_SurgeryItems> SurgeryItems { get; }

        public IList<SurgeryWithDetail_SurgeryProcedures> SurgeryProcedures { get; }

        public IList<SurgeryWithDetail_SurgeryStaffs> SurgeryStaffs { get; }

        public SurgeryWithDetail_Theatre Theatre { get; set; }

        public double? TheatreTotalCost { get; set; }

        [MapTo("_partition")]
        public string Partition { get; set; }

        public SurgeryWithDetail()
        {
            Id = ObjectId.GenerateNewId();
        }
        public SurgeryWithDetail(string partition)
        {
            Id = ObjectId.GenerateNewId();
            Partition = partition;
            Procedure = new SurgeryWithDetail_Procedure { Code = "XXX", Name = "test surgery" };
            Surgeon = new SurgeryWithDetail_Surgeon { Code = "9000", FirstName = "Ana", LastName = "popescu", Title = "Mrs" };
        }
    }

    public class SurgeryWithDetail_BodySide : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class SurgeryWithDetail_Patient : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }

    public class SurgeryWithDetail_Procedure : EmbeddedObject
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class SurgeryWithDetail_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }
    }

    public class SurgeryWithDetail_SurgeryItems : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpiryDate { get; set; }

        public string LotNumber { get; set; }

        public int? Quantity { get; set; }

        public string SerialNumber { get; set; }
    }

    public class SurgeryWithDetail_SurgeryProcedures : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class SurgeryWithDetail_SurgeryStaffs : EmbeddedObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<SurgeryWithDetail_SurgeryStaffs_SurgeryStaffTimings> SurgeryStaffTimings { get; }

        public double? TheatreStaffRate { get; set; }
    }

    public class SurgeryWithDetail_SurgeryStaffs_SurgeryStaffTimings : EmbeddedObject
    {
        public DateTimeOffset? EndTime { get; set; }

        public DateTimeOffset? StartTime { get; set; }
    }

    public class SurgeryWithDetail_Theatre : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

}
