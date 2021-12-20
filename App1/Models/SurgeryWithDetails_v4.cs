using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Realms;

namespace App1.Models
{
    public class SurgeryWithDetails_v4 : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        public string BodySide { get; set; }

        public string Duration { get; set; }

        public SurgeryWithDetails_v4_Patient Patient { get; set; }

        public SurgeryWithDetails_v4_Procedure Procedure { get; set; }

        public string Reference { get; set; }

        public SurgeryWithDetails_v4_Surgeon Surgeon { get; set; }

        public string TenantId { get; set; }

        public SurgeryWithDetails_v4_Theatre Theatre { get; set; }

        public double? TheatreTotalCost { get; set; }

        [MapTo("_partition")]
        public string Partition { get; set; }

        [MapTo("message")]
        public string Message { get; set; }


        public SurgeryWithDetails_v4()
        {
            Id = ObjectId.GenerateNewId();
        }
        public SurgeryWithDetails_v4(string partition)
        {
            Id = ObjectId.GenerateNewId();
            Partition = partition;
            Procedure = new SurgeryWithDetails_v4_Procedure { Code = "XXX", Name = "test surgery" };
            Surgeon = new SurgeryWithDetails_v4_Surgeon { Code = "9000", FirstName = "Ana", LastName = "popescu", Title = "Mrs" };
        }

    }

    public class SurgeryWithDetails_v4_Patient : EmbeddedObject
    {
        public string FullName { get; set; }
        public string PatientIdentificationNumber { get; set; }
    }
    public class SurgeryWithDetails_v4_Procedure : EmbeddedObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class SurgeryWithDetails_v4_Surgeon : EmbeddedObject
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
    public class SurgeryWithDetails_v4_Theatre : EmbeddedObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
