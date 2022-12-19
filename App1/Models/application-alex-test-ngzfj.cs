//using MongoDB.Bson;
//using Realms;
//using System;
//using System.Collections.Generic;

//namespace App1.Models
//{
//    public class Surgery2 : RealmObject
//    {
//        [MapTo("_id")]
//        [PrimaryKey]
//        public ObjectId? Id { get; set; }
//        public IList<Surgery2_Consumables> Consumables { get; }
//        public string CreatedBy { get; set; }
//        public DateTimeOffset? CreatedOn { get; set; }
//        public DateTimeOffset? FinishTime { get; set; }
//        public string ID { get; set; }
//        public IList<Implant> Implants { get; }
//        public bool? IsSynced { get; set; }
//        public IList<Item2> Items { get; }
//        public string Note { get; set; }
//        public IList<Surgery2_People> People { get; }
//        public IList<Surgery2_SecondaryProcedures> SecondaryProcedures { get; }
//        public IList<Surgery2_Staff> Staff { get; }
//        public DateTimeOffset? StartTime { get; set; }
//        [MapTo("_partition")]
//        public string Partition { get; set; }
//    }
//    public class Surgery2_Consumables : EmbeddedObject
//    {
//        public string ID { get; set; }
//        public string Name { get; set; }
//    }
//    public class Surgery2_People : EmbeddedObject
//    {
//        [MapTo("city")]
//        public string City { get; set; }

//        [MapTo("name")]
//        public string Name { get; set; }
//    }

//    public class Item2 : EmbeddedObject
//    {
//        [MapTo("name")]
//        public string Name { get; set; }
//        [MapTo("quantity")]
//        public int? Quantity { get; set; }
//    }

//    public class Surgery2_SecondaryProcedures : EmbeddedObject
//    {
//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public string ID { get; set; }

//        public string ProcedureId { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }
//    }

//    public class Surgery2_Staff : EmbeddedObject
//    {
//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public string ID { get; set; }

//        public bool? IsActive { get; set; }

//        public string StaffId { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }
//    }

//    public class Implant : RealmObject
//    {
//        [MapTo("_id")]
//        [PrimaryKey]
//        public ObjectId? Id { get; set; }
//        public string BodySideId { get; set; }
//        public Implant_BodySite BodySite { get; set; }
//        public string Description { get; set; }
//        public string ID { get; set; }
//        public bool? IsActive { get; set; }
//        public string ItemCode { get; set; }
//        public int? RatioToBase { get; set; }
//        public bool? RequiresExpiryDate { get; set; }
//        public bool? RequiresLotNumber { get; set; }
//        public bool? RequiresSerialNumber { get; set; }
//        public string SupplierItemCode { get; set; }
//        public string UnitOfMeasure { get; set; }
//        public double? UnitPrice { get; set; }
//        [MapTo("_partition")]
//        public string Partition { get; set; }
//    }
//    public class Implant_BodySite : EmbeddedObject
//    {
//        public string Code { get; set; }
//        public string Description { get; set; }
//        public string ID { get; set; }
//        public bool? IsActive { get; set; }
//    }
//}