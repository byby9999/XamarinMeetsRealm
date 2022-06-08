using App1.Models;
using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static App1.Business.Configurations;

namespace App1.Business
{
    public static class SurgeryBusiness
    {
        //these do not support .Take(x)
        public static List<Surgery> GetSurgeryList(this Realm realm)
        {
            return realm.All<Surgery>().OrderBy(s => s.Id).ToList();
        }

        public static void RemoveSurgery(this Realm realm, ObjectId objectId)
        {       
            var surgeryToRemove = realm.Find<Surgery>(objectId);
            if (surgeryToRemove != null)
            {
                realm.Write(() =>
                {
                    realm.Remove(surgeryToRemove);
                });
            } 
        }

        public static void EditSurgery(this Realm realm, ObjectId objectId, string newName, int version = 1)
        {
            var surgery = realm.Find<Surgery>(objectId);
            if (surgery != null)
            {
                var procedure = surgery.Procedure;
                var newProcedure = new Surgery_Procedure();
                newProcedure.ID = "XX";

                realm.Write(() =>
                {
                    surgery.Procedure = newProcedure;
                });
            }
                  
        }

        public static void AddSurgery(this Realm realm, string name, string partition, int version = 1)
        {
            var surgeryV1 = new Surgery();
            surgeryV1.Id = ObjectId.GenerateNewId();
            surgeryV1.Surgeon = new Surgery_Surgeon() { ID = name };
            surgeryV1.Procedure = new Surgery_Procedure() { ID = name };

            realm.Write(() =>
            {
                realm.Add(surgeryV1);
            });
        }
    }
}

