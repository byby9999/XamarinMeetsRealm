using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;

namespace App1.Models
{
    //Version description
    //Based on v2, applied these modifications: (destructive changes)
    //- Lines field: changed type from int? to string, value gets converted to string
    //- Patient field: renamed to "Client"
    //TODO: Update model here

    public class Surgery_v3 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public Surgery_v3_BodySide BodySide { get; set; }
        public Surgery_v3_Client Client { get; set; }
        public string Lines { get; set; }
        public Surgery_v3_Procedure Procedure { get; set; }
        public string Reference { get; set; }
        public Surgery_v3_Surgeon Surgeon { get; set; }
        public IList<Surgery_v3_SurgeryItems> SurgeryItems { get; }
        public IList<Surgery_v3_SurgeryProcedures> SurgeryProcedures { get; }
        public IList<Surgery_v3_SurgeryStaffs> SurgeryStaffs { get; }
        public Surgery_v3_Theatre Theatre { get; set; }
        public double? TheatreTotalCost { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("message")]
        public string Message { get; set; }

        [MapTo("v")]
        public int? V { get; set; }

        public Surgery_v3()
        {

        }
        public Surgery_v3(string name, string partitionValue, string bodySideDesc)
        {
            Procedure = new Surgery_v3_Procedure("XTEST123", name);
            BodySide = new Surgery_v3_BodySide { Code = "A", Description = bodySideDesc };
            Id = ObjectId.GenerateNewId();
            Lines = "41";
            Client = new Surgery_v3_Client { PatientIdentificationNumber = "ASDF" };
            Reference = "XXXTEST";
            Partition = partitionValue;
            TheatreTotalCost = 23;
            Theatre = new Surgery_v3_Theatre { Code = "X1", Description = "Test" };
            Surgeon = new Surgery_v3_Surgeon { LastName = "Abraham", FirstName = "Alex", Code = "xxx.x/x-x_x", Title = "Mr" };
        }
    }

    public class Surgery_v3_BodySide : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Surgery_v3_Client : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }

    public class Surgery_v3_Procedure : EmbeddedObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Surgery_v3_Procedure()
        {

        }
        public Surgery_v3_Procedure(string code)
        {
            this.Code = code;
        }
        public Surgery_v3_Procedure(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }

    public class Surgery_v3_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }

    public class Surgery_v3_SurgeryItems : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public int? Quantity { get; set; }
        public string SerialNumber { get; set; }
    }

    public class Surgery_v3_SurgeryProcedures : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Surgery_v3_SurgeryStaffs : EmbeddedObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Surgery_v3_SurgeryStaffs_SurgeryStaffTimings> SurgeryStaffTimings { get; }
        public double? TheatreStaffRate { get; set; }
    }

    public class Surgery_v3_SurgeryStaffs_SurgeryStaffTimings : EmbeddedObject
    {
        public string EndTime { get; set; }
        public DateTimeOffset? StartTime { get; set; }
    }

    public class Surgery_v3_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
