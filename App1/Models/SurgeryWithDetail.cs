using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class SurgeryWithDetail : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public BodySide BodySide { get; set; }
        public Side Side { get; set; }

        public string Duration { get; set; }
        public int? Lines { get; set; }
        public Patient Patient { get; set; }
        public Procedure Procedure { get; set; }
        public string Reference { get; set; }
        public Surgeon Surgeon { get; set; }
        public string TenantId { get; set; }
        public Theatre Theatre { get; set; }
        public double? TheatreTotalCost { get; set; }
        [MapTo("_partition")]
        public string Partition { get; set; }

        [MapTo("message")]
        public string Message { get; set; }

        [MapTo("version")]
        public int? Version { get; set; }

        [MapTo("color")]
        public string Color { get; set; }

        [Ignored]
        public string SurgeonFullName => Surgeon.LastName + " " + Surgeon.FirstName;

        [Ignored]
        public char HasMessage { get; set; }

        [Ignored]
        public string NewVersion { get; set; }

        public SurgeryWithDetail()
        {

        }
        public SurgeryWithDetail(string name, string partitionValue, string bodySideDesc)
        {
            Procedure = new Procedure { Code = "TEST123", Name = name };
            BodySide = new BodySide { Code = "A", Description = bodySideDesc };
            Duration = "01:59:59";
            Id = ObjectId.GenerateNewId();
            Lines = 4;
            Patient = new Patient { PatientIdentificationNumber = "ASDF" };
            Reference = "XXXTEST";
            TenantId = partitionValue;
            Partition = partitionValue;
            TheatreTotalCost = 23;
            Theatre = new Theatre { Code = "X1", Description = "Test" };
            Surgeon = new Surgeon { LastName = "Abraham", FirstName = "Alex", Code = "xxx.x/x-x_x", Title = "Mr" };
        }
    }

    public class BodySide : EmbeddedObject
    {
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Side : EmbeddedObject
    {
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class Patient : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }

    public class Procedure : EmbeddedObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
    public class Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
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
}
