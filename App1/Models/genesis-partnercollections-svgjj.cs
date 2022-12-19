using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;

namespace App1.Models
{
    public class Consumable : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ItemCode { get; set; }

        public int RatioToBase { get; set; }

        public bool RequiresExpiryDate { get; set; }

        public bool RequiresLotNumber { get; set; }

        public bool RequiresSerialNumber { get; set; }

        [Required]
        public string SupplierItemCode { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }

        public double UnitPrice { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class ConsumableBarcode : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public string DeviceIdentifier { get; set; }

        public string ID { get; set; }

        public bool? IsActive { get; set; }

        public string ItemCategoryId { get; set; }

        public string ItemId { get; set; }

        public string RawData { get; set; }

        public string UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        [MapTo("_partition")]
        public string Partition { get; set; }
    }

    public class Implant : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        public string BodySideId { get; set; }

        public Implant_BodySite BodySite { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ItemCode { get; set; }

        public int RatioToBase { get; set; }

        public bool RequiresExpiryDate { get; set; }

        public bool RequiresLotNumber { get; set; }

        public bool RequiresSerialNumber { get; set; }

        [Required]
        public string SupplierItemCode { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }

        public double UnitPrice { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class ImplantBarcode : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public string DeviceIdentifier { get; set; }

        public string ID { get; set; }

        public bool? IsActive { get; set; }

        public string ItemCategoryId { get; set; }

        public string ItemId { get; set; }

        public string RawData { get; set; }

        public string UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        [MapTo("_partition")]
        public string Partition { get; set; }
    }

    public class Implant_BodySite : EmbeddedObject
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string ID { get; set; }
    }

    public class ItemBarcode : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public string DeviceIdentifier { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ItemCategoryId { get; set; }

        [Required]
        public string ItemId { get; set; }

        public string RawData { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class Laterality : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class PreferenceCard : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        public IList<PreferenceCardConsumable> Consumables { get; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ID { get; set; }

        public IList<PreferenceCardImplant> Implants { get; }

        public bool IsSynced { get; set; }

        public IList<PreferenceCardProcedurePack> ProcedurePacks { get; }

        public IList<PreferenceCardProcedure> Procedures { get; }

        public IList<PreferenceCardSite> Sites { get; }

        public IList<PreferenceCardSurgeon> Staff { get; }

        [Required]
        public string State { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class PreferenceCardConsumable : EmbeddedObject
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string ItemId { get; set; }

        public long Quantity { get; set; }
    }

    public class PreferenceCardImplant : EmbeddedObject
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string ItemId { get; set; }

        public long Quantity { get; set; }
    }

    public class PreferenceCardProcedure : EmbeddedObject
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string ProcedureId { get; set; }
    }

    public class PreferenceCardProcedurePack : EmbeddedObject
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string ProcedurePackId { get; set; }

        public long Quantity { get; set; }
    }

    public class PreferenceCardSite : EmbeddedObject
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string SiteId { get; set; }
    }

    public class PreferenceCardSurgeon : EmbeddedObject
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string SurgeonId { get; set; }
    }

    public class Procedure : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Name { get; set; }

        public bool SpecificBodySideRequired { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class ProcedurePack : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ID { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class ProductRecall : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ItemId { get; set; }

        public string LotNumber { get; set; }

        [Required]
        public string SupplierId { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class Staff : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Title { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class Surgeon : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string FirstName { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public IList<string> ProcedureIds { get; }

        public IList<ObjectId> Procedures { get; }

        [Required]
        public string SecondName { get; set; }

        public string Title { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class Surgery : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        public IList<SurgeryConsumable> Consumables { get; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? FinishTime { get; set; }

        public string GlobalServiceRelationNumber { get; set; }

        [Required]
        public string ID { get; set; }

        public IList<SurgeryImplant> Implants { get; }

        public bool? IsActive { get; set; }

        public bool IsSynced { get; set; }

        [Required]
        public string Note { get; set; }

        public string PatientIdentificationNumber { get; set; }

        public ObjectId? Procedure { get; set; }

        public string ProcedureBodySideId { get; set; }

        [Required]
        public string ProcedureId { get; set; }

        public IList<SurgeryProcedurePack> ProcedurePacks { get; }

        public IList<SurgerySecondaryProcedure> SecondaryProcedures { get; }

        public IList<SurgeryStaff> Staff { get; }

        public DateTimeOffset? StartTime { get; set; }

        [Required]
        public string State { get; set; }

        public ObjectId? Surgeon { get; set; }

        public string SurgeonId { get; set; }

        public string SurgeryScheduleId { get; set; }

        public ObjectId? Theatre { get; set; }

        public string TheatreId { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class SurgeryConsumable : EmbeddedObject
    {
        public ObjectId? Consumable { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? ExpiryDate { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ItemId { get; set; }

        public long ItemStatus { get; set; }

        public string ItemStatusReasonId { get; set; }

        public string LotNumber { get; set; }

        public string Note { get; set; }

        public long Quantity { get; set; }

        public string ScannedDeviceIdentifier { get; set; }

        public string ScannedDeviceIdentifierType { get; set; }

        public string SerialNumber { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }

    public class SurgeryImplant : EmbeddedObject
    {
        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? ExpiryDate { get; set; }

        [Required]
        public string ID { get; set; }

        public ObjectId? Implant { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ItemId { get; set; }

        public long ItemStatus { get; set; }

        public string ItemStatusReasonId { get; set; }

        public string LateralityId { get; set; }

        public string LotNumber { get; set; }

        public string Note { get; set; }

        public string ScannedDeviceIdentifier { get; set; }

        public string ScannedDeviceIdentifierType { get; set; }

        public string SerialNumber { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }

    public class SurgeryItemStatusReason : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Reason { get; set; }

        public int SurgeryItemStatus { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }

    public class SurgeryProcedurePack : EmbeddedObject
    {
        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public string ID { get; set; }

        public string Note { get; set; }

        [Required]
        public string ProcedurePackId { get; set; }

        public long Quantity { get; set; }

        public string ScannedDeviceIdentifier { get; set; }

        public string ScannedDeviceIdentifierType { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }

    public class SurgerySecondaryProcedure : EmbeddedObject
    {
        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public string ID { get; set; }

        [Required]
        public string ProcedureId { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }

    public class SurgeryStaff : EmbeddedObject
    {
        [Required]
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        public string Note { get; set; }

        [Required]
        public string StaffId { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }

    public class Theatre : RealmObject
    {
        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId? Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public string GlobalLocationNumber { get; set; }

        [Required]
        public string ID { get; set; }

        public bool IsActive { get; set; }

        public string LocationId { get; set; }

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }
    }


}