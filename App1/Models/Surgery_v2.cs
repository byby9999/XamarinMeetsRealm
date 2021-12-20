﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    //Version description : Added field Message, removed field Duration
    public class Surgery_v2 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }
        public Surgery_v2_BodySide BodySide { get; set; }
        public int? Lines { get; set; }
        public Surgery_v2_Patient Patient { get; set; }
        public Surgery_v2_Procedure Procedure { get; set; }
        public string Reference { get; set; }
        public Surgery_v2_Surgeon Surgeon { get; set; }
        public IList<Surgery_v2_SurgeryItems> SurgeryItems { get; }
        public IList<Surgery_v2_SurgeryProcedures> SurgeryProcedures { get; }
        public IList<Surgery_v2_SurgeryStaffs> SurgeryStaffs { get; }
        public Surgery_v2_Theatre Theatre { get; set; }
        public double? TheatreTotalCost { get; set; }
        public double? TheatreTotalcost { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("message")]
        public string Message { get; set; }

        public Surgery_v2()
        {
            Id = ObjectId.GenerateNewId();
        }
        public Surgery_v2(string partition)
        {
            Id = ObjectId.GenerateNewId();
            Partition = partition;
            Procedure = new Surgery_v2_Procedure { Code = "XXX", Name = "test surgery" };
            Surgeon = new Surgery_v2_Surgeon { Code = "9000", FirstName = "Ana", LastName = "popescu", Title = "Mrs" };
        }
    }
    public class Surgery_v2_BodySide : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Surgery_v2_Patient : EmbeddedObject
    {
        public string PatientIdentificationNumber { get; set; }
    }
    public class Surgery_v2_Procedure : EmbeddedObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class Surgery_v2_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
    public class Surgery_v2_SurgeryItems : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public int? Quantity { get; set; }
        public string SerialNumber { get; set; }
    }
    public class Surgery_v2_SurgeryProcedures : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class Surgery_v2_SurgeryStaffs : EmbeddedObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Surgery_v2_SurgeryStaffs_SurgeryStaffTimings> SurgeryStaffTimings { get; }
        public double? TheatreStaffRate { get; set; }
        public double? TheatreStaffrate { get; set; }
    }
    public class Surgery_v2_SurgeryStaffs_SurgeryStaffTimings : EmbeddedObject
    {
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartTime { get; set; }
    }
    public class Surgery_v2_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}