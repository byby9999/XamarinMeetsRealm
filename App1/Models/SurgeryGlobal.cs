using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    //public class SurgeryGlobal : RealmObject
    //{
    //    [PrimaryKey]
    //    [MapTo("_id")]
    //    public ObjectId? Id { get; set; }

    //    public SurgeryGlobal_BodySide BodySide { get; set; }

    //    public string Duration { get; set; }

    //    public int? Lines { get; set; }
    //    public string LinesStr { get; set; }

    //    public SurgeryGlobal_Client Client { get; set; }

    //    public SurgeryGlobal_Patient Patient { get; set; }

    //    public SurgeryGlobal_Procedure Procedure { get; set; }

    //    public string Reference { get; set; }

    //    public SurgeryGlobal_Surgeon Surgeon { get; set; }

    //    public IList<SurgeryGlobal_SurgeryItems> SurgeryItems { get; }

    //    public IList<SurgeryGlobal_SurgeryStaff> SurgeryStaff { get; }

    //    public SurgeryGlobal_Theatre Theatre { get; set; }

    //    public double? TheatreTotalCost { get; set; }

    //    [MapTo("_partition")]
    //    [Required]
    //    public string Partition { get; set; }

    //    [MapTo("message")]
    //    public string Message { get; set; }

    //    [MapTo("v")]
    //    public int? V { get; set; }

    //    public SurgeryGlobal()
    //    {
    //    }
    //    public SurgeryGlobal(string name, string partition, int v)
    //    {
    //        Id = ObjectId.GenerateNewId();
    //        Procedure = new SurgeryGlobal_Procedure("Code_124", name);
    //        BodySide = new SurgeryGlobal_BodySide { Code = "L", Description = "body-side-code" };
    //        Lines = 4;
    //        Patient = new SurgeryGlobal_Patient { PatientIdentificationNumber = "ASDF" };
    //        Reference = "R_" + name;
    //        Partition = partition;
    //        TheatreTotalCost = 231;
    //        Theatre = new SurgeryGlobal_Theatre { Code = "X1", Description = "Test" };
    //        Surgeon = new SurgeryGlobal_Surgeon { LastName = "Abraham", FirstName = "Alex", Code = "xxx.x/x-x_x", Title = "Mr" };
    //        V = v;
    //        if (v == 1)
    //        {
    //            Duration = "01:23:45";
    //        }
    //        if (v == 2)
    //        {
    //            Message = "hi from C#";
    //            Duration = null;
    //        }
    //        if (v == 3) 
    //        {
    //            LinesStr = "3";
    //            Lines = null;
    //            Client = new SurgeryGlobal_Client { PatientIdentificationNumber = "PAT_123" };
    //            Patient = null;
    //        }
    //    }
    //}

    public class SurgeryGlobal_BodySide : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class SurgeryGlobal_Patient : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }

    public class SurgeryGlobal_Procedure : EmbeddedObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public SurgeryGlobal_Procedure()
        {

        }
        public SurgeryGlobal_Procedure(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    public class SurgeryGlobal_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
    public class SurgeryGlobal_SurgeryItems : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public int? Quantity { get; set; }
        public string SerialNumber { get; set; }
    }
    public class SurgeryGlobal_SurgeryStaff : EmbeddedObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<SurgeryGlobal_SurgeryStaff_SurgeryStaffTimings> SurgeryStaffTimings { get; }
        public double? TheatreStaffRate { get; set; }
    }
    public class SurgeryGlobal_SurgeryStaff_SurgeryStaffTimings : EmbeddedObject
    {
        public string EndTime { get; set; }
        public string StartTime { get; set; }
    }
    public class SurgeryGlobal_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class SurgeryGlobal_Client : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }
}