using App1.Models;
using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Business
{
    public static class SurgeryBusiness 
    {
        //these do not support .Take(x)
        public static List<Surgery> GetSurgeryList(this Realm realm) 
        {
            return realm.All<Surgery>().OrderBy(s => s.Surgeon.LastName).ToList();
        }
        public static List<Surgery_v2> GetSurgeryList_v2(this Realm realm)
        {
            return realm.All<Surgery_v2>().OrderBy(s => s.Surgeon.LastName).ToList();

        }
        public static List<Surgery_v3> GetSurgeryList_v3(this Realm realm)
        {
            return realm.All<Surgery_v3>().OrderBy(s => s.Surgeon.LastName).ToList();
        }

        public static int CountSurgeries(this Realm realm, int version)
        {
            switch (version)
            {
                case 3:
                    return realm.All<Surgery_v3>().Count();

                case 2:
                    return realm.All<Surgery_v2>().Count();

                case 1:
                    return realm.All<Surgery>().Count();

                default:
                    throw new Exception("Data version not supported.");


            }
        }
        public static List<Surgery> GetDisplayModels(this Realm medicalRealm, int version = 1)
        {
            List<Surgery> displayModels;
            switch (version)
            {
                case 3:
                    {
                        var surgeryListV3 = medicalRealm.GetSurgeryList_v3();
                        displayModels = DisplayModels.GetFrom(surgeryListV3);

                        break;
                    }
                case 2:
                    {
                        var surgeryListV2 = medicalRealm.GetSurgeryList_v2();
                        displayModels = DisplayModels.GetFrom(surgeryListV2);

                        break;
                    }
                case 1:
                    {
                        var surgeryList = medicalRealm.GetSurgeryList();
                        displayModels = DisplayModels.GetFrom(surgeryList);

                        break;
                    }

                default:
                    throw new Exception("Data version not supported.");
            }

            return displayModels.ToList();
        }

        public static void RemoveSurgery(this Realm realm, ObjectId objectId, int version = 1) 
        {
            switch (version)
            {
                case 3:
                    {
                        var surgeryToRemove = realm.Find<Surgery_v3>(objectId);
                        if (surgeryToRemove != null)
                        {
                            realm.Write(() =>
                            {
                                realm.Remove(surgeryToRemove);
                            });
                        }
                        break;
                    }
                case 2:
                    {
                        var surgeryToRemove = realm.Find<Surgery_v2>(objectId);
                        if (surgeryToRemove != null)
                        {
                            realm.Write(() =>
                            {
                                realm.Remove(surgeryToRemove);
                            });
                        }
                        break;
                    }
                case 1:
                    {
                        var surgeryToRemove = realm.Find<Surgery>(objectId);
                        if (surgeryToRemove != null)
                        {
                            realm.Write(() =>
                            {
                                realm.Remove(surgeryToRemove);
                            });
                        }
                        break;
                    }

                default:
                    throw new Exception("Data version not supported.");
            }
        }

        public static void EditSurgery(this Realm realm, ObjectId objectId, string newName, int version = 1)
        {
            switch (version)
            {
                case 3:
                    var surgery3 = realm.Find<Surgery_v3>(objectId);
                    if (surgery3 != null)
                    {
                        var procedure = surgery3.Procedure;
                        var newProcedure = new Surgery_v3_Procedure(procedure.Code, newName);

                        realm.Write(() =>
                        {
                            surgery3.Procedure = newProcedure;
                        });
                    }
                    break;
                case 2:
                    var surgery2 = realm.Find<Surgery_v2>(objectId);
                    if (surgery2 != null)
                    {
                        var procedure = surgery2.Procedure;
                        var newProcedure = new Surgery_v2_Procedure(procedure.Code, newName);
                        
                        realm.Write(() =>
                        {
                            surgery2.Procedure = newProcedure;
                        });
                    }
                    break;
                case 1:
                    var surgery = realm.Find<Surgery>(objectId);
                    if (surgery != null)
                    {
                        var procedure = surgery.Procedure;
                        var newProcedure = new Surgery_Procedure(procedure.Code, newName);
                        
                        realm.Write(() =>
                        {
                            surgery.Procedure = newProcedure;
                        });
                    }
                    break;

                default:
                    throw new Exception("Data version not supported.");
            }
        }

        public static void AddSurgery(this Realm realm, string name, string partition, int version = 1) 
        {
            switch (version)
            {
                case 3:
                    var surgeryV3 = new Surgery_v3(name, partition, "bodyside-3");

                    realm.Write(() =>
                    {
                        realm.Add(surgeryV3);
                    });
                    break;
                case 2:
                    var surgeryV2 = new Surgery_v2(name, partition, "bodyside-2");

                    realm.Write(() =>
                    {
                        realm.Add(surgeryV2);
                    });
                    break;
                case 1:
                    var surgeryV1 = new Surgery(name, partition, "bodyside-1");

                    realm.Write(() =>
                    {
                        realm.Add(surgeryV1);
                    });
                    break;

                default:
                    throw new Exception("Data version not supported.");
            }
        }

        public static string RandomName() 
        {
            Random r = new Random();
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string randomName = "surgery-";

            for (int i = 0; i < 4; i++)
            {
                randomName += letters[r.Next(0, 26)];
            }
            return randomName;
        }
    }
}

