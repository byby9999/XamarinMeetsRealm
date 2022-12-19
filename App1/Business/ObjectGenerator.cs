using App1.Models;
using System;
using MongoDB.Bson;

namespace App1.Business
{
    public static class ObjectGenerator
    {
        private const string TENANT = "PARTNERCOLLECTIONS";
        public const string IMPLANT = "B148C594-2613-425F-9A8B-434F9AD99C55";
        public const string CONSUMABLE = "35149BF6-6351-4F2F-AD64-64D00CAD1481";

        public static int Base = 1;

        public static ImplantBarcode NewImplantBarcode()
        {
            var implantBarcode = new ImplantBarcode
            {
                Partition = TENANT,
                Id = ObjectId.GenerateNewId(),
                ID = $"imlpantbarcode_{Base}",
                DeviceIdentifier = $"test_{Base}",
                IsActive = true,
                ItemCategoryId = IMPLANT,
                ItemId = $"test_{Base}",
                CreatedBy = "alex test",
                UpdatedBy = "alex test",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            return implantBarcode;
        }

        public static ConsumableBarcode NewConsumableBarcode()
        {
            var consumableBarcode = new ConsumableBarcode
            {
                Partition = TENANT,
                Id = ObjectId.GenerateNewId(),
                ID = $"consumablebarcode_{Base}",
                DeviceIdentifier = $"test_{Base}",
                IsActive = true,
                ItemCategoryId = CONSUMABLE,
                ItemId = $"test_{Base}",
                CreatedBy = "alex test",
                UpdatedBy = "alex test",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };
            Base++;

            return consumableBarcode;
        }

        public static ItemBarcode NewItemBarcode(string category)
        {
            var itemBarcodeToAdd = new ItemBarcode
            {
                Partition = TENANT,
                Id = ObjectId.GenerateNewId(),
                ID = $"itembarcode_{Base}",
                DeviceIdentifier = $"test_{Base}",
                IsActive = true,
                ItemCategoryId = category,
                ItemId = $"test_{Base}",
                CreatedBy = "alex test",
                UpdatedBy = "alex test",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };
            Base++;

            return itemBarcodeToAdd;
        }
    }
}
