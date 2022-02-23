using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    public class SurgeryGlobal : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        public SurgeryGlobal_BodySide BodySide { get; set; }

        public string Duration { get; set; }

        public int? Lines { get; set; }

        public SurgeryGlobal_Patient Patient { get; set; }

        public SurgeryGlobal_Procedure Procedure { get; set; }

        public string Reference { get; set; }

        public SurgeryGlobal_Surgeon Surgeon { get; set; }

        public IList<SurgeryGlobal_SurgeryItems> SurgeryItems { get; }

        public IList<SurgeryGlobal_SurgeryStaff> SurgeryStaff { get; }

        public SurgeryGlobal_Theatre Theatre { get; set; }

        public double? TheatreTotalCost { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("v")]
        public int? V { get; set; }
    }

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
}