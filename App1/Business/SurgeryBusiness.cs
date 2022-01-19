using App1.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Business
{
    public class SurgeryBusiness 
    {
        public static List<Surgery> GetSurgeryList(Realm realm) 
        {
            return realm.All<Surgery>().OrderBy(s => s.Surgeon.LastName).ToList();

        }
        public static List<Surgery_v2> GetSurgeryList_v2(Realm realm)
        {
            return realm.All<Surgery_v2>().OrderBy(s => s.Surgeon.LastName).ToList();

        }
        public static List<Surgery_v3> GetSurgeryList_v3(Realm realm)
        {
            return realm.All<Surgery_v3>().OrderBy(s => s.Surgeon.LastName).ToList();

        }
    }
}

