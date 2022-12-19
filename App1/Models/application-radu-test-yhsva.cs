//using System;
//using System.Collections.Generic;
//using Realms;
//using MongoDB.Bson;

//namespace App1.Models 
//{
//    using System;
//    using System.Collections.Generic;
//    using Realms;
//    using MongoDB.Bson;

//    public class Consumable : RealmObject
//    {
//        [MapTo("_id")]
//        [PrimaryKey]
//        public ObjectId? Id { get; set; }

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

//    public class Consumable_v2 : RealmObject
//    {
//        [MapTo("_id")]
//        [PrimaryKey]
//        public ObjectId? Id { get; set; }

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

//    public class Surgery : RealmObject
//    {
//        [MapTo("_id")]
//        [PrimaryKey]
//        public ObjectId? Id { get; set; }

//        public IList<Surgery_Consumables> Consumables { get; }

//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public DateTimeOffset? FinishTime { get; set; }

//        public string GlobalServiceRelationNumber { get; set; }

//        public string ID { get; set; }

//        public bool? IsActive { get; set; }

//        public bool? IsSynced { get; set; }

//        public string Note { get; set; }

//        public string PatientIdentificationNumber { get; set; }

//        public string ProcedureId { get; set; }

//        public IList<Surgery_SecondaryProcedures> SecondaryProcedures { get; }

//        public IList<Surgery_Staff> Staff { get; }

//        public DateTimeOffset? StartTime { get; set; }

//        public string State { get; set; }

//        public string SurgeonId { get; set; }

//        public string SurgeryScheduleId { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string TheatreId { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }

//        [MapTo("_partition")]
//        public string Partition { get; set; }
//    }

//    public class Surgery_Consumables : EmbeddedObject
//    {
//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public string ID { get; set; }

//        public bool? IsActive { get; set; }

//        public string ItemId { get; set; }

//        public long? ItemStatus { get; set; }

//        public string Note { get; set; }

//        public long? Quantity { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }
//    }

//    public class Surgery_SecondaryProcedures : EmbeddedObject
//    {
//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public string ID { get; set; }

//        public string ProcedureId { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }
//    }

//    public class Surgery_Staff : EmbeddedObject
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

//    public class Surgery_v2 : RealmObject
//    {
//        [MapTo("_id")]
//        [PrimaryKey]
//        public ObjectId? Id { get; set; }

//        public IList<Surgery_v2_Consumables> Consumables { get; }

//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public DateTimeOffset? FinishTime { get; set; }

//        public string GlobalServiceRelationNumber { get; set; }

//        public string ID { get; set; }

//        public bool? IsActive { get; set; }

//        public bool? IsSynced { get; set; }

//        public string MyTest { get; set; }

//        public string Note { get; set; }

//        public string PatientIdentificationNumber { get; set; }

//        public string ProcedureId { get; set; }

//        public IList<Surgery_v2_SecondaryProcedures> SecondaryProcedures { get; }

//        public IList<Surgery_v2_Staff> Staff { get; }

//        public DateTimeOffset? StartTime { get; set; }

//        public string Status { get; set; }

//        public string SurgeonId { get; set; }

//        public string SurgeryScheduleId { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string TheatreId { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }

//        [MapTo("_partition")]
//        public string Partition { get; set; }
//    }

//    public class Surgery_v2_Consumables : EmbeddedObject
//    {
//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public string ID { get; set; }

//        public bool? IsActive { get; set; }

//        public string ItemId { get; set; }

//        public long? ItemStatus { get; set; }

//        public string Note { get; set; }

//        public long? Quantity { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }
//    }

//    public class Surgery_v2_SecondaryProcedures : EmbeddedObject
//    {
//        public string CreatedBy { get; set; }

//        public DateTimeOffset? CreatedOn { get; set; }

//        public string ID { get; set; }

//        public string ProcedureId { get; set; }

//        public int? SyncErrorCode { get; set; }

//        public string UpdatedBy { get; set; }

//        public DateTimeOffset? UpdatedOn { get; set; }
//    }

//    public class Surgery_v2_Staff : EmbeddedObject
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
//}