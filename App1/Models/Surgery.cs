using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;

namespace App1.Models
{
    public class Surgery : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        public Surgery_BodySide BodySide { get; set; }

        public string Duration { get; set; }

        public int? Lines { get; set; }

        public Surgery_Patient Patient { get; set; }

        public Surgery_Procedure Procedure { get; set; }

        public string Reference { get; set; }

        public Surgery_Surgeon Surgeon { get; set; }

        public IList<Surgery_SurgeryItems> SurgeryItems { get; }

        public IList<Surgery_SurgeryProcedures> SurgeryProcedures { get; }

        public IList<Surgery_SurgeryStaffs> SurgeryStaffs { get; }

        public Surgery_Theatre Theatre { get; set; }

        public double? TheatreTotalcost { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
        
        [MapTo("v")]
        public int? V { get; set; }


        [Ignored]
        public string SurgeonFullName => Surgeon.LastName + " " + Surgeon.FirstName;

        [Ignored]
        public string Extra { get; set; }

        [Ignored]
        public string NewVersion { get; set; }

        public Surgery()
        {

        }
        public Surgery(string name, string partitionValue, string bodySideDesc)
        {
            Procedure = new Surgery_Procedure { Code = "TEST123", Name = name };
            BodySide = new Surgery_BodySide { Code = "A", Description = bodySideDesc };
            Duration = "01:59:59";
            Id = ObjectId.GenerateNewId();
            Lines = 4;
            Patient = new Surgery_Patient { PatientIdentificationNumber = "ASDF" };
            Reference = "XXXTEST";
            Partition = partitionValue;
            TheatreTotalcost = 23;
            Theatre = new Surgery_Theatre { Code = "X1", Description = "Test" };
            Surgeon = new Surgery_Surgeon { LastName = "Abraham", FirstName = "Alex", Code = "xxx.x/x-x_x", Title = "Mr" };
        }
    }

    public class Surgery_BodySide : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class Surgery_Patient : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }

    public class Surgery_Procedure : EmbeddedObject
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class Surgery_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }
    }

    public class Surgery_SurgeryItems : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpiryDate { get; set; }

        public string LotNumber { get; set; }

        public int? Quantity { get; set; }

        public string SerialNumber { get; set; }
    }

    public class Surgery_SurgeryProcedures : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class Surgery_SurgeryStaffs : EmbeddedObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<Surgery_SurgeryStaffs_SurgeryStaffTimings> SurgeryStaffTimings { get; }

        public double? TheatreStaffRate { get; set; }

        public double? TheatreStaffrate { get; set; }
    }

    public class Surgery_SurgeryStaffs_SurgeryStaffTimings : EmbeddedObject
    {
        public DateTimeOffset? EndTime { get; set; }

        public DateTimeOffset? StartTime { get; set; }
    }

    public class Surgery_Theatre : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

}
